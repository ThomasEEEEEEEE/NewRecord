using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;
using NewRecord_Backend.Models;
using Xamarin.Forms;

namespace NewRecord_Backend.ViewModels
{
    public class ViewChallengeViewModel : INotifyPropertyChanged
    {
        public class ScoreUser : INotifyPropertyChanged
        {
            private string username;
            public string Username
            {
                get { return username; }
                set
                {
                    username = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Username"));
                }
            }
            private double bestscore;
            public double BestScore
            {
                get { return bestscore; }
                set
                {
                    bestscore = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BestScore"));
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

        iDBAccess DBAccess;
        public ViewChallengeViewModel(int ChallengeID)
        {
            DBAccess = new AzureDBAccess();

            
            Challenge challenge = DBAccess.GetChallenge(ChallengeID);
            RecordName = challenge.RecordName;
            GoalScore = challenge.GoalScore;
            EndDate = challenge.EndDate;

            ViewChallenge = challenge;

            Participants = new ListViewModel<ScoreUser>();
            foreach (User user in challenge.Participants)
            {
                Record rec = DBAccess.GetRecordFromUser(user.ID, RecordName);
                ScoreUser su = new ScoreUser() { Username = user.Username, BestScore = rec.BestScore };
                Participants.ListView.Add(su);
            }
        }

        public async void ForfeitPressed()
        {
            bool conf = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to forfeit this challenge?", "Yes", "No");
            
            if (conf)
                DBAccess.ForfeitChallenge(AzureDBAccess.ID, ViewChallenge);
        }

        private Challenge ViewChallenge;

        private string recordname;
        public string RecordName
        {
            get
            {
                return recordname;
            }
            set
            {
                recordname = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecordName"));
            }
        }

        private double goalscore;
        public double GoalScore
        {
            get
            {
                return goalscore;
            }
            set
            {
                goalscore = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GoalScore"));
            }
        }

        private DateTime enddate;
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

        private ListViewModel<ScoreUser> participants;
        public ListViewModel<ScoreUser> Participants
        {
            get
            {
                return participants;
            }
            set
            {
                participants = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Participants"));
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
