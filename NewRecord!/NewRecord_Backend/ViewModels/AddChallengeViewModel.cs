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
    public class AddChallengeViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        INavigation Navigation;
        public AddChallengeViewModel(INavigation navigation)
        {
            DBAccess = new AzureDBAccess();
            Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
            EndDate = DateTime.Now;
            SelectedFriends = new List<User>();
            Navigation = navigation;
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
            //Non null record name
            if (String.IsNullOrWhiteSpace(RecordName))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter a record name", "OK");
                return;
            }
            
            //At least two participants
            if (SelectedFriends.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert("Error", "A challenge must have at least two participants", "OK");
                return;
            }

            //No more than 5 participants
            if (SelectedFriends.Count >= 5)
            {
                Application.Current.MainPage.DisplayAlert("Error", "There can only be 5 participants per challenge", "OK");
                return;
            }

            //Record must exist and must not be private
            Record chalrec = DBAccess.GetRecordFromUser(AzureDBAccess.ID, RecordName);
            if (chalrec == null || chalrec.Privacy == PrivacySettings.PRIVATE)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You do not have a record by this name", "OK");
                return;
            }
            if (chalrec.Success == SuccessInfo.LARGER && chalrec.BestScore >= GoalScore)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You already have a score better than " + GoalScore.ToString(), "OK");
                return;
            }
            if (chalrec.Success == SuccessInfo.SMALLER && chalrec.BestScore <= GoalScore)
            {
                Application.Current.MainPage.DisplayAlert("Error", "You already have a score better than " + GoalScore.ToString(), "OK");
                return;
            }

            //All participants must have record and have it not be private
            foreach (User participant in SelectedFriends)
            {
                Record partrec = DBAccess.GetRecordFromUser(participant.ID, RecordName);
                if (partrec == null || partrec.Privacy == PrivacySettings.PRIVATE || partrec.Success != chalrec.Success)
                {
                    Application.Current.MainPage.DisplayAlert("Error", "Not all participants own this record", "OK");
                    return;
                }

                if (chalrec.Success == SuccessInfo.LARGER && partrec.BestScore >= GoalScore)
                {
                    Application.Current.MainPage.DisplayAlert("Error", "One or more participants already have a score better than " + GoalScore.ToString(), "OK");
                    return;
                }
                if (chalrec.Success == SuccessInfo.SMALLER && partrec.BestScore <= GoalScore)
                {
                    Application.Current.MainPage.DisplayAlert("Error", "One or more participants already have a score better than " + GoalScore.ToString(), "OK");
                    return;
                }
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

            foreach (User participant in SelectedFriends)
            {
                DBNotification notification = new DBNotification(AzureDBAccess.ID, participant.ID, NotificationType.CHALLENGE_REQUEST, challengeid);
                DBAccess.SendNotification(notification);
            }

            Application.Current.MainPage.DisplayAlert("Success", "Challenge Successfully Created", "OK");
            Navigation.PopModalAsync();
        }

        #region Properties
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
