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
//Display goals
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
            Images.ListView.Add("nr_logo.png");
            Images.ListView.Add("bench_press.png");
            Images.ListView.Add("swimming.png");
            Images.ListView.Add("basketball.png");
            Images.ListView.Add("book.png");
            Images.ListView.Add("bowling.png");
            Images.ListView.Add("camera.png");
            Images.ListView.Add("canoe.png");
            Images.ListView.Add("car.png");
            Images.ListView.Add("computer.png");
            Images.ListView.Add("cup.png");
            Images.ListView.Add("cycling.png");
            Images.ListView.Add("dumbbell.png");
            Images.ListView.Add("football.png");
            Images.ListView.Add("golf.png");
            Images.ListView.Add("medal.png");
            Images.ListView.Add("money.png");
            Images.ListView.Add("pencil.png");
            Images.ListView.Add("running.png");
            Images.ListView.Add("soccer.png");
            Images.ListView.Add("top3.png");
            Images.ListView.Add("volleyball.png");
        }

        public void PlusGoalPressed()
        {
            AddGoalScreenVisible = !AddGoalScreenVisible;
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

            AddGoalScreenVisible = false;
            GoalScore = 0;
            EndDate = DateTime.Now;
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
        private bool addgoalscreenvisible;
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

        public bool AddGoalScreenVisible
        {
            get { return addgoalscreenvisible; }
            set
            {
                addgoalscreenvisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AddGoalScreenVisible"));
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
