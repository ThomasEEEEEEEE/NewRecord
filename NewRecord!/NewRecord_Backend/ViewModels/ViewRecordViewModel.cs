using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;
using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using Xamarin.Forms;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace NewRecord_Backend.ViewModels
{
    public class ViewRecordViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        iFileAccess FileAccess;
        public ViewRecordViewModel(string recordname)
        {
            DBAccess = new AzureDBAccess();
            FileAccess = new JsonFileAccess();

            if (AzureDBAccess.ID == -1)
                ViewRecord = FileAccess.GetRecord(recordname);
            else
                ViewRecord = DBAccess.GetRecordFromUser(AzureDBAccess.ID, recordname);
            History = new ListViewModel<RecordItem>(ViewRecord.RecordHistory);
            Goals = new ListViewModel<Goal>(ViewRecord.Goals);

            EndDate = DateTime.Now;

            //CheckForExpiredGoals();
            PopulateChart();
        }

        async void CheckForExpiredGoals()
        {
            List<Goal> ExpiredGoals = new List<Goal>();
            string alert = "";
            for (int i = 0; i < ViewRecord.Goals.Count; ++i)
            {
                if (Goals.ListView[i].EndDate < DateTime.Now)
                {
                    ExpiredGoals.Add(Goals.ListView[i]);
                    alert += ViewRecord.Goals[i].GoalScore.ToString() + " before " + ViewRecord.Goals[i].EndDate.ToString() + "\n";
                }
            }

            if (String.IsNullOrWhiteSpace(alert))
                return;

            await Application.Current.MainPage.DisplayAlert("You have failed to reach the following goals in time", alert, "OK");
            ExpiredGoals.ForEach(x => Goals.ListView.Remove(x));

            if (AzureDBAccess.ID == -1)
                FileAccess.RemoveGoalsFromRecord(ViewRecord.Name, ExpiredGoals);
            else
                DBAccess.RemoveMultipleGoalsFromRecord(AzureDBAccess.ID, ViewRecord.Name, ExpiredGoals);
        }

        async void CheckForAchievedGoals(double newscore)
        {
            string alert = "";
            List<Goal> AchievedGoals = new List<Goal>();

            for (int i = 0; i < Goals.ListView.Count; ++i)
            {
                Goal goal = Goals.ListView[i];
                if (ViewRecord.Success == SuccessInfo.LARGER && goal.GoalScore <= newscore)
                {
                    AchievedGoals.Add(goal);
                    alert += goal.GoalScore.ToString() + " by " + goal.EndDate + "\n";
                }
                else if (ViewRecord.Success == SuccessInfo.SMALLER && goal.GoalScore >= newscore)
                {
                    alert += goal.GoalScore.ToString() + " by " + goal.EndDate + "\n";
                    AchievedGoals.Add(goal);
                }
            }
            if (String.IsNullOrWhiteSpace(alert))
                return;

            await Application.Current.MainPage.DisplayAlert("You've achieved the following goals!", alert, "OK");

            if (AzureDBAccess.ID == -1)
                FileAccess.RemoveGoalsFromRecord(ViewRecord.Name, AchievedGoals);
            else
                DBAccess.RemoveMultipleGoalsFromRecord(AzureDBAccess.ID, ViewRecord.Name, AchievedGoals);
        }

        public LineChart PopulateChart()
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = 0; i < ViewRecord.RecordHistory.Count; ++i)
            {
                if (i == 0 || i == ViewRecord.RecordHistory.Count - 1)
                {
                    entries.Add(new ChartEntry((float)(ViewRecord.RecordHistory[i].Score))
                    {
                        Label = ViewRecord.RecordHistory[i].DateAchieved.ToShortDateString(),
                        ValueLabel = ViewRecord.RecordHistory[i].Score.ToString(),
                        TextColor = SKColor.Parse("#000000"),
                        Color = SKColor.Parse("#FF0000"),
                    });
                }
                else
                {
                    entries.Add(new ChartEntry((float)(ViewRecord.RecordHistory[i].Score))
                    {
                        Color = SKColor.Parse("#FF0000")
                    });
                }
            }
            RecordChart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#EEEEEE"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 15,
                PointSize = 35,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            return RecordChart;
        }

        public void UpdateButtonClicked(string recordname, double newscore)
        {
            if (ViewRecord.Success == SuccessInfo.LARGER && ViewRecord.BestScore >= newscore)
                return;
            if (ViewRecord.Success == SuccessInfo.SMALLER && ViewRecord.BestScore <= newscore)
                return;

            if (AzureDBAccess.ID == -1)
                FileAccess.UpdateRecord(recordname, newscore);
            else
                DBAccess.UpdateRecord(AzureDBAccess.ID, recordname, newscore);

            History.ListView.Add(new RecordItem(newscore, DateTime.Now));

            PopulateChart();

            //CheckForAchievedGoals(newscore);
        }

        public void EditNameButtonClicked(string newname)
        {
            if (AzureDBAccess.ID == -1)
                FileAccess.EditRecordName(ViewRecord.Name, newname);
            else
                DBAccess.EditRecordName(AzureDBAccess.ID, ViewRecord.Name, newname);

            ViewRecord.Name = newname;
        }

        public void EditPrivacyButtonClicked(PrivacySettings privacy)
        {
            if (AzureDBAccess.ID == -1)
                FileAccess.EditRecordPrivacy(ViewRecord.Name, privacy);
            else
                DBAccess.EditRecordPrivacy(AzureDBAccess.ID, ViewRecord.Name, privacy);

            ViewRecord.Privacy = privacy;
        }

        public void AddGoalButtonClicked(double goalscore)
        {
            Goal goal = new Goal(goalscore, EndDate);
            if (AzureDBAccess.ID == -1)
                FileAccess.AddGoalToRecord(ViewRecord.Name, goal);
            else
                DBAccess.AddGoalToRecord(AzureDBAccess.ID, ViewRecord.Name, goal);

            Goals.ListView.Add(goal);
        }

        private ListViewModel<RecordItem> history;
        private ListViewModel<Goal> goals;
        private LineChart recordchart;
        private Record viewrecord;
        private DateTime enddate;

        public ListViewModel<RecordItem> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                PropertyChanged(this, new PropertyChangedEventArgs("History"));
            }
        }

        public ListViewModel<Goal> Goals
        {
            get
            {
                return goals;
            }
            set
            {
                goals = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Goals"));
            }
        }

        public LineChart RecordChart
        {
            get
            {
                return recordchart;
            }
            set
            {
                recordchart = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecordChart"));
            }
        }

        public Record ViewRecord 
        {
            get
            {
                return viewrecord;
            }
            set
            {
                viewrecord = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ViewRecord"));
            }
        }

        public DateTime EndDate
        {
            get
            {
                return enddate;
            }
            set
            {
                enddate = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
            }
        }

        #region PropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        void OnPropertyChanged([CallerMemberName] string propertyname = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
