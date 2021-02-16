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

namespace NewRecord_Backend.ViewModels
{
    public class AddChallengeViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public AddChallengeViewModel()
        {
            DBAccess = new AzureDBAccess();
            Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
            EndDate = DateTime.Now;
            SelectedFriends = new List<User>();
        }

        public void CheckboxChecked(User user, bool boxchecked)
        {
            if (boxchecked)
                SelectedFriends.Add(user);
            else
                SelectedFriends.Remove(user);
        }

        public void CreateButtonClicked()
        {
            //No more than 5 participants
            if (SelectedFriends.Count > 5)
                return;

            //Record must exist and must not be private
            Record chalrec = DBAccess.GetRecordFromUser(AzureDBAccess.ID, RecordName);
            if (chalrec == null || chalrec.Privacy == PrivacySettings.PRIVATE)
                return;

            foreach (User participant in SelectedFriends)
            {
                Record partrec = DBAccess.GetRecordFromUser(participant.ID, RecordName);
                if (partrec == null || partrec.Privacy == PrivacySettings.PRIVATE || partrec.Success != chalrec.Success) //Also need to check if any participants have beat goal already
                    return;
            }
            //Add self to participants
            List<User> participants = new List<User>(SelectedFriends);
            participants.Add(DBAccess.GetUser(AzureDBAccess.ID));

            Challenge challenge = new Challenge();
            challenge.RecordName = RecordName;
            challenge.GoalScore = GoalScore;
            challenge.Success = chalrec.Success;
            challenge.EndDate = EndDate;
            challenge.Participants = participants;
            int challengeid = DBAccess.CreateChallenge(challenge);

            //Need to add changes to CHALLENGE_PARTICIPANTS

            foreach (User participant in SelectedFriends)
            {
                DBNotification notification = new DBNotification(AzureDBAccess.ID, participant.ID, NotificationType.CHALLENGE_REQUEST, challengeid);
                DBAccess.SendNotification(notification);
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
        private ListViewModel<User> friends;
        public ListViewModel<User> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Friends"));
            }
        }
        public List<User> SelectedFriends { get; set; }
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
