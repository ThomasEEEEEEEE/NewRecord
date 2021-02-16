using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;

namespace NewRecord_Backend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocialPage : ContentPage
    {
        SocialViewModel vm;
        public SocialPage()
        {
            InitializeComponent();

            vm = new SocialViewModel(Navigation);
            BindingContext = vm;
        }

        private void FLbutton_Clicked(object sender, EventArgs e)
        {
            vm.FriendsButtonPressed();
        }

        private void AddFriendButton_Clicked(object sender, EventArgs e)
        {
            vm.AddFriendButtonPressed();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            vm.SearchButtonClicked();
        }

        private void AddChallengeButton_Clicked(object sender, EventArgs e)
        {
            vm.AddChallengeButtonClicked();
        }
    }
}