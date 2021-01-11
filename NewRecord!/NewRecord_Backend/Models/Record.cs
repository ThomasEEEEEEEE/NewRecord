using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    
    public enum PrivacySettings
    {
        PUBLIC,
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
        public Record(string name, double score)
        {
            Name = name;
            RecordHistory = new List<RecordItem>();
            RecordHistory.Add(new RecordItem(score, DateTime.Now));
        }
        public string Name { get; set; }
        public string SelectedImage { get; set; }
        public PrivacySettings Privacy { get; set; }
        public SuccessInfo Success { get; set; }
        public List<RecordItem> RecordHistory { get; set; }

        //This property returns the best score of the record's RecordHistory
        public double BestScore 
        { 
            get
            {
                if (Success == SuccessInfo.LARGER)
                    return RecordHistory.Max(x => x.Score);
                else
                    return RecordHistory.Min(x => x.Score);
            }
        }
    }
}
