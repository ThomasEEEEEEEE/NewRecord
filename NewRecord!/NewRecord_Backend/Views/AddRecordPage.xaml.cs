using NewRecord_Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.Models;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace NewRecord_Backend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecordPage : ContentPage
    {
        AddRecordViewModel vm;
        public AddRecordPage()
        {
            InitializeComponent();
            vm = new AddRecordViewModel(Navigation);
            
            BindingContext = vm;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            Record newrecord = new Record();
            newrecord.Name = NameEntry.Text.Trim();
            newrecord.RecordHistory.Add(new RecordItem(Convert.ToDouble(BestScoreEntry.Text), DateTime.Now));

            newrecord.SelectedImage = ImageCarousel.CurrentItem.ToString();
            newrecord.Success = SuccessPicker.SelectedItem.ToString() == "Larger" ? SuccessInfo.LARGER : SuccessInfo.SMALLER;
            newrecord.Goals = vm.Goals.ListView.ToList();
            /*if (PrivacyPicker.SelectedItem.ToString() == "Public")
                newrecord.Privacy = PrivacySettings.PUBLIC;
            else if (PrivacyPicker.SelectedItem.ToString() == "Private")
                newrecord.Privacy = PrivacySettings.PRIVATE;
            else
                newrecord.Privacy = PrivacySettings.FRIENDSONLY;*/
            newrecord.Privacy = (PrivacySettings)PrivacyPicker.SelectedIndex;

            //vm.AddButtonPressed(newrecord);
            Navigation.PopModalAsync();
        }

        private void AddGoalButton_Clicked(object sender, EventArgs e)
        {
            Goal goal = new Goal(Convert.ToDouble(GoalScoreEntry.Text), EndDatePicker.Date);
            //vm.AddGoalButtonPressed(goal);
        }
    }
}