using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.Models;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using NewRecord_Backend.ViewModels;
using System.IO;
using Newtonsoft.Json;

namespace NewRecord_Backend.Views
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

            vm.UpdateButtonClicked(RecordName.Text, Convert.ToDouble(score));
        }

        async private void EditNameButton_Clicked(object sender, EventArgs e)
        {
            string newname = await DisplayPromptAsync("Change Record Name", "What is the new name for this record?", "Change", keyboard: Keyboard.Text);

            if (newname == null)
                return;
            vm.EditNameButtonClicked(newname.Trim());
            
            //RecordName.Text = newname;
        }

        async private void EditPrivacyButton_Clicked(object sender, EventArgs e)
        {
            string[] privacyoptions = { "Public", "Private", "Friends Only" };
            string choice = await DisplayActionSheet("Change Privacy", "Cancel", null, privacyoptions);

            if (choice == "Cancel")
                return;

            vm.EditPrivacyButtonClicked((PrivacySettings)Enum.Parse(typeof(PrivacySettings), choice));

            //PrivacyInfo.Text = privacy.ToString();
        }

        private void AddGoalButton_Clicked(object sender, EventArgs e)
        {
            vm.AddGoalButtonClicked(Convert.ToDouble(GoalScoreEntry.Text));
        }
    }
}
