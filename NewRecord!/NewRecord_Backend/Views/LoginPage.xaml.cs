using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using NewRecord_Backend.Database;

namespace NewRecord_Backend.Views
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
            Navigation.PushModalAsync(new MainTabbedPage());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "LocalRecords.json");
        }
    }
}