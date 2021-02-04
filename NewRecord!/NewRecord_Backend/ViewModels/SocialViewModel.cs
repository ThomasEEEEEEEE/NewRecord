using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using Timer = System.Timers.Timer;

namespace NewRecord_Backend.ViewModels
{
    public class SocialViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public SocialViewModel()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                Timer timer = new Timer(5000);
                timer.Elapsed += (object source, ElapsedEventArgs e) =>
                {
                    List<DBNotification> notifs = DBAccess.GetNotifications(AzureDBAccess.ID);
                    foreach (DBNotification notif in notifs)
                    {
                        switch (notif.NotificationType)
                        {
                            case NotificationType.FRIEND_REQUEST:
                                //Application.Current.MainPage.DisplayAlert("Friend Request Received", "From " + notif.SenderID, "OK");
                                break;
                            case NotificationType.CHALLENGE_REQUEST:
                                break;
                        }
                    }
                };
                timer.AutoReset = true;
                timer.Enabled = true;
            }).ConfigureAwait(false);
            DBAccess = new AzureDBAccess();
            FriendsListVisible = true;

            //Challenges = new ListViewModel<Challenge>(DBAccess.GetUserChallenges(AzureDBAccess.ID));
            //Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
        }

        private ListViewModel<Challenge> challenges;
        public ListViewModel<Challenge> Challenges
        {
            get
            {
                return challenges;
            }
            set
            {
                challenges = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Challenges"));
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

        private bool friendslistvisible;
        public bool FriendsListVisible
        {
            get
            {
                return friendslistvisible;
            }
            set
            {
                friendslistvisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FriendsListVisible"));
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
