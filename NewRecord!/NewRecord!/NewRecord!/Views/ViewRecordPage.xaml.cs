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

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecordPage : ContentPage
    {
        ViewRecordViewModel vm = new ViewRecordViewModel();
        string FileName = "LocalRecords.json";
        public ViewRecordPage(Record record)
        {
            InitializeComponent();
            RecordImage.Source = record.SelectedImage;
            RecordName.Text = record.Name;
            PrivacyInfo.Text = record.Privacy.ToString();
            SuccessSettings.Text = record.Success.ToString();

            List<ChartEntry> entries = new List<ChartEntry>()
            {
            };

            for (int i = 0; i < record.RecordHistory.Count; ++i)
            {
                if (i == 0 || i == record.RecordHistory.Count - 1)
                {
                    entries.Add(new ChartEntry((float)(record.RecordHistory[i].Score))
                    {
                        Label = record.RecordHistory[i].DateAchieved.ToShortDateString(),
                        ValueLabel = record.RecordHistory[i].Score.ToString(),
                        ValueLabelColor = SKColor.Parse("#FFFFFF"),
                        Color = SKColor.Parse("#5A3333")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry((float)(record.RecordHistory[i].Score))
                    {
                        //Label = "04/24/19" + i.ToString(),
                        Color = SKColor.Parse("#5A3333")
                    });
                }
            }

            var chart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#452222"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 5,
                PointSize = 12,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            RecChart.Chart = chart;


            record.RecordHistory.ForEach(x => vm.History.ListView.Add(x));
            record.Goals.ForEach(x => vm.Goals.ListView.Add(x));
            BindingContext = vm;
        }

        async private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            string score = await DisplayPromptAsync("New Record?", "Enter your new record", placeholder: "(eg 56.32)", keyboard: Keyboard.Numeric);

            if (score == null)
                return;

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            if (records == null)
                records = new List<Record>();

            RecordItem newitem = new RecordItem(Convert.ToDouble(score), DateTime.Now);
            records.Find(x => x.Name.ToLower() == RecordName.Text.ToLower()).RecordHistory.Add(newitem);

            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);

            vm.History.ListView.Add(newitem);
            //Update chart
        }

        async private void EditNameButton_Clicked(object sender, EventArgs e)
        {
            string newname = await DisplayPromptAsync("Change Record Name", "What is the new name for this record?", "Change", keyboard: Keyboard.Text);

            if (newname == null)
                return;
            newname = newname.Trim();

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            //Check for duplicate names
            if (records.Find(x => x.Name.ToLower() == newname.ToLower()) != null)
            {
                await DisplayAlert("Error", "Duplicate Record Name", "OK");
                return;
            }

            records.Find(x => x.Name.ToLower() == RecordName.Text.ToLower()).Name = newname;

            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);

            RecordName.Text = newname;
        }

        async private void EditPrivacyButton_Clicked(object sender, EventArgs e)
        {
            string[] privacyoptions = { "Public", "Private", "Friends Only" };
            string choice = await DisplayActionSheet("Change Privacy", "Cancel", null, privacyoptions);

            if (choice == "Cancel")
                return;

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            PrivacySettings privacy;
            if (choice == "Public")
                privacy = PrivacySettings.PUBLIC;
            else if (choice == "Private")
                privacy = PrivacySettings.PRIVATE;
            else
                privacy = PrivacySettings.FRIENDSONLY;

            records.Find(x => x.Name.ToLower() == RecordName.Text.ToLower()).Privacy = privacy;

            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);

            PrivacyInfo.Text = privacy.ToString();
        }

        private void AddGoalButton_Clicked(object sender, EventArgs e)
        {
            Goal goal = new Goal();
            goal.GoalScore = Convert.ToDouble(GoalScoreEntry.Text);
            goal.StartDate = StartDatePicker.Date;
            goal.EndDate = EndDatePicker.Date;
            vm.Goals.ListView.Add(goal);

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            records.Find(x => x.Name.ToLower() == RecordName.Text.ToLower()).Goals.Add(goal);

            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);

        }
    }
}
