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
        ListViewModel<RecordItem> History = new ListViewModel<RecordItem>();
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

            for (int i = 0; i < 50; ++i)
            {
                entries.Add(new ChartEntry(i * i / 4)
                {
                    Label = "04/24/19" + i.ToString(),
                    //ValueLabel = i.ToString(),
                    Color = SKColor.Parse("#5A2222")
                });
            }

            var chart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#250101"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 5,
                PointSize = 12,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            RecChart.Chart = chart;


            record.RecordHistory.ForEach(x => History.ListView.Add(x));
            BindingContext = History;
        }

        async private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            string score = await DisplayPromptAsync("New Record?", "Enter your new record", placeholder: "(eg 56.32)", keyboard: Keyboard.Numeric);

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            List<Record> records;
            string contents = File.ReadAllText(FilePath + FileName);
            records = JsonConvert.DeserializeObject<List<Record>>(contents);

            if (records == null)
                records = new List<Record>();

            records.Find(x => x.Name.ToLower() == RecordName.Text.ToLower()).RecordHistory.Add(new RecordItem(Convert.ToDouble(score), DateTime.Now));

            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath + FileName, newcontents);
        }
    }
}
