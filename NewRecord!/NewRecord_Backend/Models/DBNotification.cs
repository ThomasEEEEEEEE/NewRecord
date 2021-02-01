using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    //Unfinished
    public enum NotificationType
    {
        FRIEND_REQUEST = 0,
        FRIEND_REQUEST_ACCEPTED,
        FRIEND_REQUEST_DENIED,
        CHALLENGE_REQUEST,
        CHALLENGE_ACCEPTED,
        CHALLENGE_DENIED
    }
    public class DBNotification
    {
        public DBNotification(int sendid, int recid, NotificationType type, int challid = -1)
        {
            SenderID = sendid;
            ReceiverID = recid;
            NotificationType = type;
            ChallengeID = challid;
        }

        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public int ChallengeID { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
