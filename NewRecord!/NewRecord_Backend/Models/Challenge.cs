using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public class Challenge
    {
        public int ChallengeID { get; set; }
        public string RecordName { get; set; }
        public SuccessInfo Success { get; set; }
        public double GoalScore { get; set; }
        public DateTime EndDate { get; set; }
        public List<User> Participants { get; set; }
    }
}
