using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;
using NewRecord_Backend.Models;

namespace NewRecord_Backend.OfficialViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecordPage : ContentPage
    {
        ViewRecordViewModel vm;
        public ViewRecordPage(string recordname)
        {
            InitializeComponent();

            vm = new ViewRecordViewModel(recordname);
            BindingContext = vm;
        }
        async private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            string score = await DisplayPromptAsync("New Record?", "Enter your new record", placeholder: "(eg 56.32)", keyboard: Keyboard.Numeric);

            if (score == null)
                return;

            vm.UpdateButtonClicked(Convert.ToDouble(score));
        }

        async private void EditNameButton_Clicked(object sender, EventArgs e)
        {
            string newname = await DisplayPromptAsync("Change Record Name", "What is the new name for this record?", "Change", keyboard: Keyboard.Text);

            if (newname == null || String.IsNullOrWhiteSpace(newname))
                return;
            vm.EditNameButtonClicked(newname.Trim());
        }

        async private void EditPrivacyButton_Clicked(object sender, EventArgs e)
        {
            string[] privacyoptions = { "Public", "Private", "Friends Only" };
            string choice = await DisplayActionSheet("Change Privacy", "Cancel", null, privacyoptions);

            if (choice == "Cancel")
                return;

            PrivacySettings privacy;
            switch (choice)
            {
                case "Public":
                    privacy = PrivacySettings.PUBLIC;
                    break;
                case "Private":
                    privacy = PrivacySettings.PRIVATE;
                    break;
                case "Friends Only":
                    privacy = PrivacySettings.FRIENDSONLY;
                    break;
                default:
                    privacy = PrivacySettings.PRIVATE; //Should never happen
                    break;
            }
            vm.EditPrivacyButtonClicked(privacy);
        }
        private void PlusGoal_Clicked(object sender, EventArgs e)
        {
            vm.PlusGoalPressed();
        }

        private void AddGoal_Clicked(object sender, EventArgs e)
        {
            vm.AddGoalButtonClicked();
        }
    }
}