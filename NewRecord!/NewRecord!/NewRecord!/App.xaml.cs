//using NewRecord.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewRecord_
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NewRecord_Backend.Views.LoginPage();
            MainPage = new NewRecord_Backend.OfficialViews.LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
