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
    public partial class SocialPage : ContentPage
    {
        SocialViewModel vm;
        public SocialPage()
        {
            InitializeComponent();
            vm = new SocialViewModel(Navigation);

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }
        private void CListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.ChallengeItemTapped(e.ItemIndex);
        }

        private void FLbutton_Clicked(object sender, EventArgs e)
        {
            vm.FriendsButtonPressed();
        }

        private void AddFriendButton_Clicked(object sender, EventArgs e)
        {
            vm.AddFriendButtonPressed();
        }

        private void FListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.FriendsItemTapped(e.ItemIndex);
        }
        private void AddButton_Clicked(object sender, EventArgs e)
        {
            vm.AddChallengeButtonClicked();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            vm.SearchButtonClicked();
        }

        private void NavigateToSettings(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SettingsPage());
        }

        private void NavigateToAbout(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AboutPage());
        }

        private void FCListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.FChallengeItemTapped(e.ItemIndex);
        }
    }
}