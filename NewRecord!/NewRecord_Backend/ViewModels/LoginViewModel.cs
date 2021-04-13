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
using NewRecord_Backend.OfficialViews;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace NewRecord_Backend.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        INavigation Navigation;
        public LoginViewModel(INavigation navigation)
        {
            DBAccess = new AzureDBAccess();
            Navigation = navigation;

            //Populate Username and Password with last login values
            Username = Preferences.Get("LastLoginUsername", "");
            Password = Preferences.Get("LastLoginPassword", "");
            ShowSignUp = false;
        }
        public void OnAppearing()
        {
            ShowLoading = false;
        }
        public void LoginButtonPressed()
        {
            if (String.IsNullOrWhiteSpace(Username))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter a username", "OK");
                return;
            }

            if (String.IsNullOrWhiteSpace(Password))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter a password", "OK");
                return;
            }

            ShowLoading = true;
            Task.Run(() =>
            {
                User user = DBAccess.GetUser(Username);
                if (user != null && Hashing.VerifyPassword(Password, user.PasswordHash))
                {
                    AzureDBAccess.ID = user.ID;
                    Preferences.Set("LastLoginUsername", Username);
                    Preferences.Set("LastLoginPassword", Password);

                    Device.BeginInvokeOnMainThread(() => Navigation.PushModalAsync(new MainTabbedPage()));
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ShowLoading = false;
                        Application.Current.MainPage.DisplayAlert("Error", "Invalid Username or Password", "OK");
                    });

                }
            });
        }
        public void ShowSignupPressed()
        {
            ShowSignUp = !ShowSignUp;
        }
        public void SignupButtonPressed()
        {
            if (String.IsNullOrWhiteSpace(SignUpUsername))
            {
                _ = Application.Current.MainPage.DisplayAlert("Error", "Please enter a username", "OK");
                return;
            }

            if (String.IsNullOrWhiteSpace(SignUpPassword))
            {
                _ = Application.Current.MainPage.DisplayAlert("Error", "Please enter a password", "OK");
                return;
            }

            if (DBAccess.GetUser(SignUpUsername) != null)
            {
                _ = Application.Current.MainPage.DisplayAlert("Error", "An account with that username already exists", "OK");
                return;
            }
            ShowLoading = true;
            Task.Run(() =>
            {
                User user = new User();
                user.Username = SignUpUsername;
                user.PasswordHash = Hashing.HashPassword(SignUpPassword);
                DBAccess.AddUser(user);
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowLoading = false;
                    Application.Current.MainPage.DisplayAlert("Success", "Account Successfully Created!", "OK");
                });
            });
        }

        public async void ContinueButtonPressed()
        {
            AzureDBAccess.ID = -1;
            await Navigation.PushModalAsync(new UnregisteredTabbedPage());

        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private bool showsignup;
        public bool ShowSignUp
        {
            get { return showsignup; }
            set
            {
                showsignup = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowSignUp"));
            }
        }

        private string signupusername;
        public string SignUpUsername
        {
            get
            {
                return signupusername;
            }
            set
            {
                signupusername = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SignUpUsername"));
            }
        }

        private string signuppassword;
        public string SignUpPassword
        {
            get
            {
                return signuppassword;
            }
            set
            {
                signuppassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SignUpPassword"));
            }
        }

        private bool showloading;
        public bool ShowLoading
        {
            get
            {
                return showloading;
            }
            set
            {
                showloading = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowLoading"));
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
