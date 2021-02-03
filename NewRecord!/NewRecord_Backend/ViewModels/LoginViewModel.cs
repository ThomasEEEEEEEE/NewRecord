﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;
using NewRecord_Backend.Models;
using NewRecord_Backend.Views;
using Xamarin.Forms;

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
        }
        public void LoginButtonPressed()
        {
            User user = DBAccess.GetUser(Username);
            if (Hashing.VerifyPassword(Password, user.PasswordHash))
            {
                AzureDBAccess.ID = user.ID;
                Navigation.PushModalAsync(new MainTabbedPage());
            }
            else
                Application.Current.MainPage.DisplayAlert("Error", "Invalid Username/Password", "OK");

        }
        public async void SignupButtonPressed()
        {
            User user = new User();
            user.Username = Username;
            user.PasswordHash = Hashing.HashPassword(Password);
            DBAccess.AddUser(user);
            await Application.Current.MainPage.DisplayAlert("Success", "Account Successfully Created!", "OK");
        }

        public async void ContinueButtonPressed()
        {
            bool conf = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to continue without an account?\nYou won't be able use the full functionality of the app", "Yes", "No");
            if (!conf)
                return;
            else
            {
                AzureDBAccess.ID = -1;
                MainTabbedPage page = new MainTabbedPage();
                page.Children.RemoveAt(1);
                page.Children.Add(new UnregisteredSocialPage());
                await Navigation.PushModalAsync(new MainTabbedPage());
            }    
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
