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
using NewRecord_Backend.Views;
using Timer = System.Timers.Timer;

namespace NewRecord_Backend.ViewModels
{
    public class SocialViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        INavigation Navigation;
        public SocialViewModel(INavigation navigation)
        {
            Navigation = navigation;

            System.Threading.Tasks.Task.Run(() =>
            {
                //A timer that will elapse every ten seconds and check for new notifications
                Timer timer = new Timer(10000);
                timer.Elapsed += async (object source, ElapsedEventArgs e) =>
                {
                    List<DBNotification> notifs = DBAccess.GetNotifications(AzureDBAccess.ID);
                    foreach (DBNotification notif in notifs)
                    {
                        bool acc;
                        switch (notif.NotificationType)
                        {
                            case NotificationType.FRIEND_REQUEST:
                                acc = false;
                                await Device.InvokeOnMainThreadAsync(async () =>
                                {
                                    acc = await Application.Current.MainPage.DisplayAlert("Friend Request Received", "From " + notif.SenderID, "Accept", "Decline");
                                });

                                if (acc)
                                    DBAccess.AcceptFriendRequest(AzureDBAccess.ID, notif.SenderID);
                                DBAccess.RemoveNotification(notif);
                                break;

                            case NotificationType.CHALLENGE_REQUEST:
                                acc = false;
                                await Device.InvokeOnMainThreadAsync(async () =>
                                {
                                    acc = await Application.Current.MainPage.DisplayAlert("Challenge Request Received", "From " + notif.SenderID, "Accept", "Decline");
                                });

                                if (acc)
                                    DBAccess.AcceptChallengeRequest(notif);
                                else
                                    DBAccess.DeclineChallengeRequest(notif);

                                DBAccess.RemoveNotification(notif);
                                break;
                        }
                    }
                };
                timer.AutoReset = true;
                timer.Enabled = true;
            }).ConfigureAwait(false);
            DBAccess = new AzureDBAccess();
            FriendsListVisible = false;

            Challenges = new ListViewModel<Challenge>(DBAccess.GetUserChallenges(AzureDBAccess.ID));
            Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
            FriendRequests = new ListViewModel<User>(DBAccess.GetUserFriendRequests(AzureDBAccess.ID));
        }

        public void FriendsButtonPressed()
        {
            FriendsListVisible = !FriendsListVisible;
        }

        public async void AddFriendButtonPressed()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Send Friend Request", "Enter Their Username", "Add", "Cancel", "Username");
            if (name == null || name == "Cancel")
                return;

            User user = DBAccess.GetUser(name);

            //If user doesn't exist
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User Not Found", "OK");
                return;
            }

            //If user is already a friend
            if (DBAccess.GetUserFriends(AzureDBAccess.ID).Find(x => x.ID == user.ID) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "That user is already a friend", "OK");
                return;
            }

            //Also need to check for dup requests

            DBAccess.SendFriendRequest(AzureDBAccess.ID, user.ID);
        }

        public async void SearchButtonClicked()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Search Public Records", "Enter Their Username", "Search", "Cancel", "Username");

            if (name == null || name == "Cancel")
                return;

            User user = DBAccess.GetUser(name);

            //If user doesn't exist
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User Not Found", "OK");
                return;
            }

            _ = Navigation.PushModalAsync(new PublicRecordsPage(user.ID));
        }

        public void AddChallengeButtonClicked()
        {
            _ = Navigation.PushModalAsync(new AddChallengePage());
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

        private ListViewModel<User> friendrequests;
        public ListViewModel<User> FriendRequests
        {
            get
            {
                return friendrequests;
            }
            set
            {
                friendrequests = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FriendRequests"));
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
