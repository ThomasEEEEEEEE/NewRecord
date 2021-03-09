using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;

namespace NewRecord_Backend.OfficialViews
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

        private void ShowSignup(object sender, EventArgs e)
        {
            vm.ShowSignupPressed();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }
    }
}