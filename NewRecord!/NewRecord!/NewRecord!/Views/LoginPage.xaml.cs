﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using NewRecord_Database;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            //Debug user
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            AzureDBAccess.ID = 1;
            Navigation.PushModalAsync(new MainTabbedPage());
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            AzureDBAccess.ID = -1;
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "LocalRecords.json");
            Navigation.PushModalAsync(new MainTabbedPage());
        }
    }
}