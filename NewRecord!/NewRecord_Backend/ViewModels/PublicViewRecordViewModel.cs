using Microcharts;
using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewRecord_Backend.ViewModels
{
    class PublicViewRecordViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public PublicViewRecordViewModel(int userid, string recordname)
        {
            DBAccess = new AzureDBAccess();

            Record ViewRecord;

            ViewRecord = DBAccess.GetRecordFromUser(userid, recordname);
            History = new ListViewModel<RecordItem>(ViewRecord.RecordHistory);
            Goals = new ListViewModel<Goal>(ViewRecord.Goals);

            RecordName = ViewRecord.Name;
            Success = ViewRecord.Success;
            SelectedImage = ViewRecord.SelectedImage;

            switch (ViewRecord.Privacy)
            {
                case PrivacySettings.PUBLIC:
                    Privacy = "Public";
                    break;
                case PrivacySettings.PRIVATE:
                    Privacy = "Private";
                    break;
                case PrivacySettings.FRIENDSONLY:
                    Privacy = "Friends Only";
                    break;
                default:
                    Privacy = "Error";
                    break;
            }

            PopulateChart();
        }

        public LineChart PopulateChart()
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = 0; i < History.ListView.Count; ++i)
            {
                if (i == 0 || i == History.ListView.Count - 1)
                {
                    entries.Add(new ChartEntry((float)(History.ListView[i].Score))
                    {
                        Label = History.ListView[i].DateAchieved.ToShortDateString(),
                        ValueLabel = History.ListView[i].Score.ToString(),
                        TextColor = SKColor.Parse("#FFFFFF"),
                        Color = SKColor.Parse("#FF0000"),
                        ValueLabelColor = SKColor.Parse("#FFFFFF")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry((float)(History.ListView[i].Score))
                    {
                        Color = SKColor.Parse("#FF0000")
                    });
                }
            }
            RecordChart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                BackgroundColor = SKColor.Parse("#2e0000"),
                PointMode = PointMode.Circle,
                LabelTextSize = 30,
                LineSize = 15,
                PointSize = 35,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal
            };
            return RecordChart;
        }

        #region Properties
        private ListViewModel<RecordItem> history;
        private ListViewModel<Goal> goals;
        private LineChart recordchart;
        private string recordname;
        private string selectedimage;
        private string privacy;
        private SuccessInfo Success; //No property

        public ListViewModel<RecordItem> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                PropertyChanged(this, new PropertyChangedEventArgs("History"));
            }
        }

        public ListViewModel<Goal> Goals
        {
            get
            {
                return goals;
            }
            set
            {
                goals = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Goals"));
            }
        }

        public LineChart RecordChart
        {
            get
            {
                return recordchart;
            }
            set
            {
                recordchart = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecordChart"));
            }
        }

        public string RecordName
        {
            get { return recordname; }
            set
            {
                recordname = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecordName"));
            }
        }
        public string SelectedImage
        {
            get { return selectedimage; }
            set
            {
                selectedimage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        public string Privacy
        {
            get { return privacy; }
            set
            {
                privacy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Privacy"));
            }
        }
        #endregion
        #region PropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        void OnPropertyChanged([CallerMemberName] string propertyname = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
