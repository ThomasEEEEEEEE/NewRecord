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

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecordPage : ContentPage
    {
        AddRecordViewModel model = new AddRecordViewModel();
        string FileName = "LocalRecords.json";
        public AddRecordPage()
        {
            InitializeComponent();
            model.Images = new ListViewModel<string>();
            model.Goals = new ListViewModel<Goal>();

            model.Images.ListView.Add("bench_press.png");
            model.Images.ListView.Add("swimming.png");
            BindingContext = model;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            if (records == null)
                records = new List<Record>();
            else if (records.Find(x => x.Name.ToLower() == NameEntry.Text.ToLower()) != null)
            {
                DisplayAlert("Error", "You already have a record of this name\nTry deleting it or naming this something else", "OK");
                return;
            }

            Record rec = new Record(NameEntry.Text.Trim(), Convert.ToDouble(BestScoreEntry.Text));
            rec.SelectedImage = ImageCarousel.CurrentItem.ToString();
            rec.Success = SuccessPicker.SelectedItem.ToString() == "Larger" ? SuccessInfo.LARGER : SuccessInfo.SMALLER;
            rec.Goals = model.Goals.ListView.ToList();
            if (PrivacyPicker.SelectedItem.ToString() == "Public")
                rec.Privacy = PrivacySettings.PUBLIC;
            else if (PrivacyPicker.SelectedItem.ToString() == "Private")
                rec.Privacy = PrivacySettings.PRIVATE;
            else
                rec.Privacy = PrivacySettings.FRIENDSONLY;

            records.Add(rec);
            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);
            Navigation.PopModalAsync();
        }

        private void AddGoalButton_Clicked(object sender, EventArgs e)
        {
            Goal goal = new Goal();
            goal.GoalScore = Convert.ToDouble(GoalScoreEntry.Text);
            goal.StartDate = StartDatePicker.Date;
            goal.EndDate = EndDatePicker.Date;
            model.Goals.ListView.Add(goal);

        }
    }
}