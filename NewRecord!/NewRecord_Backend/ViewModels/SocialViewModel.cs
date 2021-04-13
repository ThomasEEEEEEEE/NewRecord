using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Essentials;
using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using NewRecord_Backend.OfficialViews;
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

            //A timer that will elapse every fifteen seconds and check for new notifications
            Timer timer = new Timer(15000);
            timer.Elapsed += async (object source, ElapsedEventArgs e) =>
            {
                timer.Stop();
                List<DBNotification> notifs = DBAccess.GetNotifications(AzureDBAccess.ID);
                foreach (DBNotification notif in notifs)
                {
                    bool acc;
                    bool autoacc;
                    User sender = DBAccess.GetUser(notif.SenderID);
                    switch (notif.NotificationType)
                    {
                        case NotificationType.FRIEND_REQUEST:
                            acc = false;
                            autoacc = Preferences.Get("FRToggled", false);
                            if (!autoacc)
                            {
                                await Device.InvokeOnMainThreadAsync(async () =>
                                {
                                    acc = await Application.Current.MainPage.DisplayAlert("Friend Request Received", "From " + sender.Username, "Accept", "Decline");
                                });
                            }
                            if (acc || autoacc)
                            {
                                DBAccess.AcceptFriendRequest(AzureDBAccess.ID, notif.SenderID);
                                Friends.ListView.Add(sender);
                            }
                            else
                                DBAccess.DeclineFriendRequest(notif);

                            DBAccess.RemoveNotification(notif);
                            break;

                        case NotificationType.CHALLENGE_REQUEST:
                            acc = false;
                            autoacc = Preferences.Get("CHToggled", false);

                            Challenge chal = DBAccess.GetChallenge(notif.ChallengeID.Value);

                            if (!autoacc)
                            {
                                await Device.InvokeOnMainThreadAsync(async () =>
                                {
                                        //acc = await Application.Current.MainPage.DisplayAlert("Challenge Request Received", "From " + notif.SenderID, "Accept", "Decline");
                                        acc = await Application.Current.MainPage.DisplayAlert(
                                        String.Format("Challenge Request Received From {0} for record \"{1}\"", sender.Username, chal.RecordName),
                                        String.Format("They challenge you to reach {0} by {1}", chal.GoalScore, chal.EndDate.ToShortDateString()),
                                        "Accept", "Decline");
                                });
                            }
                            if (acc || autoacc)
                            {
                                DBAccess.AcceptChallengeRequest(notif);
                                Challenges.ListView.Add(DBAccess.GetChallenge(notif.ChallengeID.Value));
                            }
                            else
                                DBAccess.DeclineChallengeRequest(notif);

                            DBAccess.RemoveNotification(notif);
                            break;
                    }
                }
                timer.Start();
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            DBAccess = new AzureDBAccess();
            FriendsListVisible = false;

            Challenges = new ListViewModel<Challenge>(DBAccess.GetUserChallenges(AzureDBAccess.ID));
            Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
        }

        public void OnAppearing()
        {
            Challenges = new ListViewModel<Challenge>(DBAccess.GetUserChallenges(AzureDBAccess.ID));
            FinishedChallenges = new ListViewModel<Challenge>(DBAccess.GetFinishedChallenges(AzureDBAccess.ID));
            Friends = new ListViewModel<User>(DBAccess.GetUserFriends(AzureDBAccess.ID));
        }

        public void FriendsButtonPressed()
        {
            FriendsListVisible = !FriendsListVisible;
        }

        public async void AddFriendButtonPressed()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Send Friend Request", "Enter Their Username", "Add", "Cancel", "Username");
            if (String.IsNullOrWhiteSpace(name) || name == "Cancel")
                return;

            User user = DBAccess.GetUser(name);

            //If user doesn't exist
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User Not Found", "OK");
                return;
            }

            //If user is already a friend
            if (DBAccess.CheckForFriendship(AzureDBAccess.ID, user.ID))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "That user is already a friend", "OK");
                return;
            }

            //If a request has already been sent
            if (DBAccess.GetNotifications(user.ID).Find(x => x.NotificationType == NotificationType.FRIEND_REQUEST && x.SenderID == AzureDBAccess.ID) != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You have already sent that user a friend request", "OK");
                return;
            }

            DBAccess.SendFriendRequest(AzureDBAccess.ID, user.ID);
        }

        public async void SearchButtonClicked()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Search Public Records", "Enter Their Username", "Search", "Cancel", "Username");

            if (String.IsNullOrWhiteSpace(name) || name == "Cancel")
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

        public void ChallengeItemTapped(int index)
        {
            _ = Navigation.PushModalAsync(new ViewChallengePage(Challenges.ListView[index].ChallengeID));
        }

        public void FChallengeItemTapped(int index)
        {
            _ = Navigation.PushModalAsync(new ViewFinishedChallengePage(FinishedChallenges.ListView[index].ChallengeID));
        }

        async public void FriendsItemTapped(int index)
        {
            bool conf = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure that you want to delete this friend?", "Yes", "No");

            if (conf)
            {
                DBAccess.RemoveFriend(AzureDBAccess.ID, Friends.ListView[index].ID);
                Friends.ListView.RemoveAt(index);
            }
        }

        #region Properties
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

        private ListViewModel<Challenge> finishedchallenges;
        public ListViewModel<Challenge> FinishedChallenges
        {
            get
            {
                return finishedchallenges;
            }
            set
            {
                finishedchallenges = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FinishedChallenges"));
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
