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

            /*string ExpiredGoals = "";
            for (int i = 0; i < ViewRecord.Goals.Count; ++i)
            {
                if (ViewRecord.Goals[i].EndDate > DateTime.Now)
                {
                    ExpiredGoals += ViewRecord.Goals[i].GoalScore.ToString() + " before " + ViewRecord.Goals[i].EndDate.ToString() + "\n";
                }
            }

            if (!String.IsNullOrWhiteSpace(ExpiredGoals))
                Application.Current.MainPage.DisplayAlert("You have failed to reach the following goals in time", ExpiredGoals, "OK");*/
        }

        void PopulateChart()
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
                        ValueLabelColor = SKColor.Parse("#FFFFFF"),
                        Color = SKColor.Parse("#5A3333")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry((float)(ViewRecord.RecordHistory[i].Score))
                    {
                        Color = SKColor.Parse("#5A3333")
                    });
                }
            }
            RecordChart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#452222"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 12,
                PointSize = 20,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
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

            /*string AchievedGoals = "";
            for (int i = 0; i < Goals.ListView.Count; ++i)
            {
                Goal goal = Goals.ListView[i];
                if (ViewRecord.Success == SuccessInfo.LARGER && goal.GoalScore <= Convert.ToDouble(score))
                {
                    AchievedGoals += goal.GoalScore.ToString() + " by " + goal.EndDate + "\n";
                }
                else if (ViewRecord.Success == SuccessInfo.SMALLER && goal.GoalScore >= Convert.ToDouble(score))
                {
                    AchievedGoals += goal.GoalScore.ToString() + " by " + goal.EndDate + "\n";
                }
            }
            if (!String.IsNullOrWhiteSpace(AchievedGoals))
                await DisplayAlert("You've achieved the following goals!", AchievedGoals, "OK");*/
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
            Goal goal = new Goal(goalscore, EndDatePicker.Date);
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
        private DatePicker enddatepicker;

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

        public DatePicker EndDatePicker
        {
            get
            {
                return enddatepicker;
            }
            set
            {
                enddatepicker = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EndDatePicker"));
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
