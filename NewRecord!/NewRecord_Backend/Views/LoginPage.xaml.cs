using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using NewRecord_Backend.Database;
using NewRecord_Backend.ViewModels;

namespace NewRecord_Backend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            vm = new LoginViewModel(Navigation);
            BindingContext = vm;
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            vm.LoginButtonPressed();
        }
        private void Signup_Clicked(object sender, EventArgs e)
        {
            vm.SignupButtonPressed();
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            vm.ContinueButtonPressed();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "LocalRecords.json");
        }
    }
}