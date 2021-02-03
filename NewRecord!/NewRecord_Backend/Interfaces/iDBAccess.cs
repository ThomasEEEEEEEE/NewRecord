using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;

namespace NewRecord_Backend.Interfaces
{
    public interface iDBAccess
    {
        void AddRecordToUser(int userid, Record record);
        void RemoveRecordFromUser(int userid, string recordname);
        void AddGoalToRecord(int userid, string recordname, Goal goal);
        void RemoveGoalFromRecord(int userid, string recordname, Goal goal);
        void RemoveMultipleGoalsFromRecord(int userid, string recordname, List<Goal> goals);
        void EditRecordName(int userid, string recordname, string newname);
        void EditRecordPrivacy(int userid, string recordname, PrivacySettings newsetting);
        void UpdateRecord(int userid, string recordname, double newscore);
        Record GetRecordFromUser(int userid, string recordname);
        List<Record> GetAllUserRecords(int userid);
        void SendNotification(DBNotification notif);
        List<DBNotification> GetNotifications(int userid);
        List<Challenge> GetUserChallenges(int userid);
        List<User> GetUserFriends(int userid);
        void CreateChallenge(Challenge challenge);
        void SendFriendRequest(int userid, int friendid);
        void AcceptFriendRequest(int userid, int friendid);
    }
}
