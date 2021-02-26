using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;

//TODO for this page: Add a absolute menu for adding goals
//Add goals and display them
//Limit to 5 goals
//Edit record creation to use values from the radio buttons
namespace NewRecord_Backend.ViewModels
{
    public class AddRecordViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        iFileAccess FileAccess;
        public AddRecordViewModel()
        {
            Images = new ListViewModel<string>();
            Goals = new ListViewModel<Goal>();
            EndDate = DateTime.Now;

            DBAccess = new AzureDBAccess();
            FileAccess = new JsonFileAccess();

            AddImages();
        }

        void AddImages()
        {
            Images.ListView.Add("bench_press.png");
            Images.ListView.Add("swimming.png");
            //Insert more images
        }

        public void AddButtonPressed()
        {
            Record record = new Record() { Name = RecordName, SelectedImage = SelectedImage };
            record.RecordHistory.Add(new RecordItem(BestScore, DateTime.Now));
            record.Goals = Goals.ListView.ToList();
            //Need to get privacy and success

            if (AzureDBAccess.ID == -1)
                FileAccess.AddRecord(record);
            else
                DBAccess.AddRecordToUser(AzureDBAccess.ID, record);
        }

        public void AddGoalButtonPressed()
        {
            Goal goal = new Goal(GoalScore, EndDate);
            Goals.ListView.Add(goal);
        }

        #region Properties
        private ListViewModel<string> images;
        private ListViewModel<Goal> goals;
        private DateTime enddate;
        private double goalscore;
        private double bestscore;
        private string recordname;
        private string selectedimage;
        private bool publicchecked;
        private bool privatechecked;
        private bool friendsonlychecked;
        private bool largerchecked;
        private bool smallerchecked;
        public ListViewModel<string> Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Images"));
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
        public DateTime EndDate
        {
            get
            {
                return enddate;
            }
            set
            {
                enddate = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
            }
        }

        public double GoalScore
        {
            get { return goalscore; }
            set
            {
                goalscore = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GoalScore"));
            }
        }

        public double BestScore
        {
            get { return bestscore; }
            set
            {
                bestscore = value;
                PropertyChanged(this, new PropertyChangedEventArgs("BestScore"));
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

        public bool PublicChecked
        {
            get { return publicchecked; }
            set
            {
                publicchecked = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PublicChecked"));
            }
        }

        public bool PrivateChecked
        {
            get { return privatechecked; }
            set
            {
                privatechecked = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PrivateChecked"));
            }
        }

        public bool FriendsonlyChecked
        {
            get { return friendsonlychecked; }
            set
            {
                friendsonlychecked = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FriendsonlyChecked"));
            }
        }

        public bool LargerChecked
        {
            get { return largerchecked; }
            set
            {
                largerchecked = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LargerChecked"));
            }
        }

        public bool SmallerChecked
        {
            get { return smallerchecked; }
            set
            {
                smallerchecked = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SmallerChecked"));
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
