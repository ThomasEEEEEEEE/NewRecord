using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public class Goal
    {
        public Goal(double score, DateTime dt)
        {
            GoalScore = score;
            EndDate = dt;
        }
        public double GoalScore { get; set; }
        //public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
