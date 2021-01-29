using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    
    public enum PrivacySettings
    {
        PUBLIC = 0,
        PRIVATE,
        FRIENDSONLY
    }
    public enum SuccessInfo
    {
        LARGER,
        SMALLER
    }
    public class Record : INotifyPropertyChanged
    {
        public Record() 
        {
            RecordHistory = new List<RecordItem>();
            Goals = new List<Goal>();
        }
        private string name;
        public string Name 
        { 
            get
            {
                return name;
            }
            set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        private string selectedimage;
        public string SelectedImage
        {
            get
            {
                return selectedimage;
            }
            set
            {
                selectedimage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        private PrivacySettings privacy;
        public PrivacySettings Privacy 
        {
            get
            {
                return privacy;
            }
            set
            {
                privacy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Privacy"));
            }
        }
        private SuccessInfo success;
        public SuccessInfo Success 
        {
            get
            {
                return success;
            }
            set
            {
                success = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Success"));
            }
        }
        public List<RecordItem> RecordHistory { get; set; }
        public List<Goal> Goals { get; set; }

        //This property returns the best score of the record's RecordHistory
        public double BestScore 
        { 
            get
            {
                if (RecordHistory.Count == 0)
                    return 0;
                else if (Success == SuccessInfo.LARGER)
                    return RecordHistory.Max(x => x.Score);
                else
                    return RecordHistory.Min(x => x.Score);
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
