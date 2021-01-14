using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public class RecordItem
    {
        public RecordItem() { }
        public RecordItem(double score, DateTime dt)
        {
            Score = score;
            DateAchieved = dt;
        }
        public double Score { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}
