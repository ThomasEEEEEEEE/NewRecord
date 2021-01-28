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

        public void AddButtonPressed(Record record)
        {
            if (AzureDBAccess.ID == -1)
                FileAccess.AddRecord(record);
            else
                DBAccess.AddRecordToUser(AzureDBAccess.ID, record);
        }

        public void AddGoalButtonPressed(Goal goal)
        {
            Goals.ListView.Add(goal);
        }

        private ListViewModel<string> images;
        private ListViewModel<Goal> goals;
        private DateTime enddate;
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
