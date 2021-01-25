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

            //if (userisloggedin) then
            //MainPage = new TabbedPage();
            //else
            MainPage = new NewRecord_Backend.Views.LoginPage();
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
