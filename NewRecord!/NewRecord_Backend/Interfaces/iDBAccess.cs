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
        List<Record> GetPublicUserRecords(int userid);
        List<Record> GetNonPrivateUserRecords(int userid);
        void SendNotification(DBNotification notif);
        List<DBNotification> GetNotifications(int userid);
        void RemoveNotification(DBNotification notification);
        List<Challenge> GetUserChallenges(int userid);
        List<Challenge> GetUserChallengesForRecord(int userid, string recordname);
        Challenge GetChallenge(int chalid);
        List<User> GetUserFriends(int userid);
        bool CheckForFriendship(int user1, int user2);
        List<User> GetUserFriendRequests(int userid);
        int CreateChallenge(Challenge challenge);
        void WinChallenge(int userid, Challenge challenge);
        void ForfeitChallenge(int userid, Challenge challenge);
        void SendFriendRequest(int userid, int friendid);
        void AcceptFriendRequest(int userid, int friendid);
        void DeclineFriendRequest(DBNotification notification);
        void RemoveFriend(int user1, int user2);
        void AcceptChallengeRequest(DBNotification notification);
        void DeclineChallengeRequest(DBNotification notification);
        void AddUser(User user);
        User GetUser(int userid);
        User GetUser(string username);
        List<Challenge> GetFinishedChallenges(int userid);
        Challenge GetFinishedChallenge(int challid);
    }
}
