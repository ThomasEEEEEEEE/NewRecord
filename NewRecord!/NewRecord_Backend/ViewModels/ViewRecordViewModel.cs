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

            Record ViewRecord;

            if (AzureDBAccess.ID == -1)
                ViewRecord = FileAccess.GetRecord(recordname);
            else
                ViewRecord = DBAccess.GetRecordFromUser(AzureDBAccess.ID, recordname);

            History = new ListViewModel<RecordItem>(ViewRecord.RecordHistory);
            Goals = new ListViewModel<Goal>(ViewRecord.Goals);

            EndDate = DateTime.Now;

            RecordName = ViewRecord.Name;
            AddGoalScreenVisible = false;
            Success = ViewRecord.Success;
            SelectedImage = ViewRecord.SelectedImage;

            switch (ViewRecord.Privacy)
            {
                case PrivacySettings.PUBLIC:
                    Privacy = "Public";
                    break;
                case PrivacySettings.PRIVATE:
                    Privacy = "Private";
                    break;
                case PrivacySettings.FRIENDSONLY:
                    Privacy = "Friends Only";
                    break;
                default:
                    Privacy = "Error";
                    break;
            }

            CheckForExpiredGoals();
            PopulateChart();
        }

        async void CheckForExpiredGoals()
        {
            List<Goal> ExpiredGoals = new List<Goal>();
            string alert = "";
            for (int i = 0; i < Goals.ListView.Count; ++i)
            {
                if (Goals.ListView[i].EndDate < DateTime.Now)
                {
                    ExpiredGoals.Add(Goals.ListView[i]);
                    alert += Goals.ListView[i].GoalScore.ToString() + " before " + Goals.ListView[i].EndDate.ToShortDateString() + "\n";
                }
            }

            if (ExpiredGoals.Count == 0)
                return;

            await Application.Current.MainPage.DisplayAlert("You have failed to reach the following goals in time", alert, "OK");
            ExpiredGoals.ForEach(x => Goals.ListView.Remove(x));

            if (AzureDBAccess.ID == -1)
                FileAccess.RemoveGoalsFromRecord(RecordName, ExpiredGoals);
            else
                DBAccess.RemoveMultipleGoalsFromRecord(AzureDBAccess.ID, RecordName, ExpiredGoals);
        }

        async void CheckForAchievedGoals(double newscore)
        {
            string alert = "";
            List<Goal> AchievedGoals = new List<Goal>();

            for (int i = 0; i < Goals.ListView.Count; ++i)
            {
                Goal goal = Goals.ListView[i];
                if (Success == SuccessInfo.LARGER && goal.GoalScore <= newscore)
                {
                    AchievedGoals.Add(goal);
                    alert += goal.GoalScore.ToString() + " by " + goal.EndDate.ToShortDateString() + "\n";
                }
                else if (Success == SuccessInfo.SMALLER && goal.GoalScore >= newscore)
                {
                    alert += goal.GoalScore.ToString() + " by " + goal.EndDate.ToShortDateString() + "\n";
                    AchievedGoals.Add(goal);
                }
            }
            if (AchievedGoals.Count == 0)
                return;

            await Application.Current.MainPage.DisplayAlert("You've achieved the following goals!", alert, "OK");
            AchievedGoals.ForEach(x => Goals.ListView.Remove(x));

            if (AzureDBAccess.ID == -1)
                FileAccess.RemoveGoalsFromRecord(RecordName, AchievedGoals);
            else
                DBAccess.RemoveMultipleGoalsFromRecord(AzureDBAccess.ID, RecordName, AchievedGoals);
        }

        public LineChart PopulateChart()
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = 0; i < History.ListView.Count; ++i)
            {
                if (i == 0 || i == History.ListView.Count - 1)
                {
                    entries.Add(new ChartEntry((float)(History.ListView[i].Score))
                    {
                        Label = History.ListView[i].DateAchieved.ToShortDateString(),
                        ValueLabel = History.ListView[i].Score.ToString(),
                        TextColor = SKColor.Parse("#FFFFFF"),
                        Color = SKColor.Parse("#FF0000"),
                        ValueLabelColor = SKColor.Parse("#FFFFFF")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry((float)(History.ListView[i].Score))
                    {
                        Color = SKColor.Parse("#FF0000")
                    });
                }
            }
            RecordChart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#2e0000"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 15,
                PointSize = 35,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            return RecordChart;
        }

        public async void UpdateButtonClicked(double newscore)
        {
            double BestScore;
            if (Success == SuccessInfo.LARGER)
                BestScore = History.ListView.Max(x => x.Score);
            else
                BestScore = History.ListView.Min(x => x.Score);

            if (Success == SuccessInfo.LARGER && BestScore >= newscore)
            {
                _ = Application.Current.MainPage.DisplayAlert("Error", "This score is not better than your current best", "OK");
                return;
            }
            if (Success == SuccessInfo.SMALLER && BestScore <= newscore)
            {
                _ = Application.Current.MainPage.DisplayAlert("Error", "This score is not better than your current best", "OK");
                return;
            }

            if (AzureDBAccess.ID == -1)
                FileAccess.UpdateRecord(recordname, newscore);
            else
                DBAccess.UpdateRecord(AzureDBAccess.ID, recordname, newscore);

            History.ListView.Add(new RecordItem(newscore, DateTime.Now));

            PopulateChart();

            //Check to see if the user won any challenges
            if (AzureDBAccess.ID != -1)
            {
                List<Challenge> chals = DBAccess.GetUserChallengesForRecord(AzureDBAccess.ID, RecordName);
                foreach (Challenge chal in chals)
                {
                    if (Success == SuccessInfo.LARGER && newscore >= chal.GoalScore)
                    {
                        DBAccess.WinChallenge(AzureDBAccess.ID, chal);
                        await Application.Current.MainPage.DisplayAlert("Challenge Won!", "You won a challenge!", "OK");
                    }
                    else if (Success == SuccessInfo.SMALLER && newscore <= chal.GoalScore)
                    {
                        DBAccess.WinChallenge(AzureDBAccess.ID, chal);
                        await Application.Current.MainPage.DisplayAlert("Challenge Won!", "You won a challenge!", "OK");
                    }
                }
            }

            CheckForAchievedGoals(newscore);
        }

        public void EditNameButtonClicked(string newname)
        {
            if (AzureDBAccess.ID == -1 && FileAccess.GetRecord(newname) != null)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You already have a record with that name", "OK");
                return;
            }
            else if (AzureDBAccess.ID != -1 && DBAccess.GetRecordFromUser(AzureDBAccess.ID, newname) != null)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You already have a record with that name", "OK");
                return;
            }

            if (AzureDBAccess.ID == -1)
                FileAccess.EditRecordName(RecordName, newname);
            else
                DBAccess.EditRecordName(AzureDBAccess.ID, RecordName, newname);

            RecordName = newname;
        }

        public void EditPrivacyButtonClicked(PrivacySettings privacy)
        {
            if (AzureDBAccess.ID == -1)
                FileAccess.EditRecordPrivacy(RecordName, privacy);
            else
                DBAccess.EditRecordPrivacy(AzureDBAccess.ID, RecordName, privacy);

            switch (privacy)
            {
                case PrivacySettings.PUBLIC:
                    Privacy = "Public";
                    break;
                case PrivacySettings.PRIVATE:
                    Privacy = "Private";
                    break;
                case PrivacySettings.FRIENDSONLY:
                    Privacy = "Friends Only";
                    break;
                default:
                    Privacy = "Error";
                    break;
            }
        }

        public void AddGoalButtonClicked()
        {
            Goal goal = new Goal(GoalScore, EndDate);
            if (AzureDBAccess.ID == -1)
                FileAccess.AddGoalToRecord(RecordName, goal);
            else
                DBAccess.AddGoalToRecord(AzureDBAccess.ID, RecordName, goal);

            Goals.ListView.Add(goal);
            AddGoalScreenVisible = false;
        }

        public void PlusGoalPressed()
        {
            AddGoalScreenVisible = !AddGoalScreenVisible;
        }

#region Properties
        private ListViewModel<RecordItem> history;
        private ListViewModel<Goal> goals;
        private LineChart recordchart;
        private string recordname;
        private string selectedimage;
        private string privacy;
        private bool addgoalscreenvisible;
        private double goalscore;
        private DateTime enddate;
        public DateTime Today; //For Minimum Date
        private SuccessInfo Success; //No property

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

        public string RecordName
        {
            get { return recordname; }
            set
            {
                recordname = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecordName"));
            }
        }
        public string SelectedImage
        {
            get { return selectedimage; }
            set
            {
                selectedimage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        public string Privacy
        {
            get { return privacy; }
            set
            {
                privacy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Privacy"));
            }
        }

        public bool AddGoalScreenVisible
        {
            get { return addgoalscreenvisible; }
            set
            {
                addgoalscreenvisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AddGoalScreenVisible"));
            }
        }

        public double GoalScore
        {
            get { return goalscore; }
            set
            {
                goalscore = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GoalScore"));
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
#endregion
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
