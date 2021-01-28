using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Record
    {
        public Record() 
        {
            RecordHistory = new List<RecordItem>();
            Goals = new List<Goal>();
        }
        /*public Record(string name, double score)
        {
            Name = name;
            RecordHistory = new List<RecordItem>();
            RecordHistory.Add(new RecordItem(score, DateTime.Now));
            Goals = new List<Goal>();
        }*/
        public string Name { get; set; }
        public string SelectedImage { get; set; }
        public PrivacySettings Privacy { get; set; }
        public SuccessInfo Success { get; set; }
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
    }
}
