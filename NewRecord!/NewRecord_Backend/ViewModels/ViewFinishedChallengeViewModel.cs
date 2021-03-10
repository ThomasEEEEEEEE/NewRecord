using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewRecord_Backend.ViewModels
{
    public class ViewFinishedChallengeViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public ViewFinishedChallengeViewModel(int ChallengeID)
        {
            DBAccess = new AzureDBAccess();

            Challenge challenge = DBAccess.GetFinishedChallenge(ChallengeID);
            RecordName = challenge.RecordName;
            GoalScore = challenge.GoalScore;
            EndDate = challenge.EndDate;

            ViewChallenge = challenge;

            Participants = new ListViewModel<User>(challenge.Participants);
        }

        #region Properties
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

        private ListViewModel<User> participants;
        public ListViewModel<User> Participants
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
