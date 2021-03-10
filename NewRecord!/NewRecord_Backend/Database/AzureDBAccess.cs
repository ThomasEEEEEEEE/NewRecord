using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using System.Data.SqlClient;

namespace NewRecord_Backend.Database
{
    public class AzureDBAccess : iDBAccess
    {
        public static int ID { get; set; }

        const string DataSource = "tcp:newrecord-server.database.windows.net";
        const string UserID = "NewRecordAdmin";
        const string Password = "Hootie8667";
        const string InitialCatalog = "NewRecordDB";
        private static string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = InitialCatalog;
                return builder.ConnectionString;
            }
        }
        void DoQuery_NoReturn(string query)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(query, con))
                {
                    con.Open();
                    comm.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        //This does not return goals or record history
        Record DoQuery_OneRecord(string query)
        {
            Record record = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            record = new Record();
                            record.Name = reader.GetString(1);
                            record.SelectedImage = reader.GetString(2);
                            record.Success = (SuccessInfo)reader.GetInt32(3);
                            record.Privacy = (PrivacySettings)reader.GetInt32(4);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return record;
        }

        User DoQuery_OneUser(string query)
        {
            User user = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            user = new User();
                            user.ID = reader.GetInt32(0);
                            user.Username = reader.GetString(1);
                            user.PasswordHash = reader.GetString(2);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return user;
        }

        //For now this does not fill in Goals or RecordHistory
        List<Record> DoQuery_MultipleRecords(string query)
        {
            List<Record> records = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        records = new List<Record>();
                        while (reader.Read())
                        {
                            Record record = new Record();
                            record.Name = reader.GetString(1);
                            record.SelectedImage = reader.GetString(2);
                            record.Success = (SuccessInfo)reader.GetInt32(3);
                            record.Privacy = (PrivacySettings)reader.GetInt32(4);
                            records.Add(record);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return records;
        }

        List<RecordItem> DoQuery_MultipleRecordItems(string query)
        {
            List<RecordItem> recorditems = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        recorditems = new List<RecordItem>();

                        while (reader.Read())
                        {
                            recorditems.Add(new RecordItem(reader.GetDouble(2), reader.GetDateTime(3)));
                        }
                        
                        recorditems.Sort((x, y) => x.DateAchieved.CompareTo(y.DateAchieved));
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return recorditems;
        }

        List<Goal> DoQuery_MultipleGoals(string query)
        {
            List<Goal> goals = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        goals = new List<Goal>();

                        while (reader.Read())
                        {
                            goals.Add(new Goal(reader.GetDouble(2), reader.GetDateTime(3)));
                        }
                        goals.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return goals;
        }

        List<DBNotification> DoQuery_MultipleNotifications(string query)
        {
            List<DBNotification> notifs = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        notifs = new List<DBNotification>();
                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            int sendid = reader.GetInt32(0);
                            int recid = reader.GetInt32(1);
                            NotificationType type = (NotificationType)reader.GetInt32(2);
                            int? challid = null;
                            if (!reader.IsDBNull(3)) 
                                challid = reader.GetInt32(3);

                            notifs.Add(new DBNotification(sendid, recid, type, challid));
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return notifs;
        }

        List<User> DoQuery_MultipleUsers(string query)
        {
            List<User> users = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        users = new List<User>();
                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            User user = new User();
                            user.ID = reader.GetInt32(0);
                            user.Username = reader.GetString(1);
                            if (reader.FieldCount == 3)
                                user.PasswordHash = reader.GetString(2);
                            users.Add(user);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return users;
        }

        Challenge DoQuery_OneChallenge(string query)
        {
            Challenge challenge = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            challenge = new Challenge();
                            challenge.ChallengeID = reader.GetInt32(0);
                            challenge.RecordName = reader.GetString(1);
                            challenge.GoalScore = reader.GetDouble(2);
                            challenge.Success = (SuccessInfo)reader.GetInt32(3);
                            challenge.EndDate = reader.GetDateTime(4);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return challenge;
        }

        List<Challenge> DoQuery_MultipleChallenges(string query)
        {
            List<Challenge> challenges = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        challenges = new List<Challenge>();
                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            Challenge chal = new Challenge();
                            chal.ChallengeID = reader.GetInt32(0);
                            chal.RecordName = reader.GetString(1);
                            chal.GoalScore = reader.GetDouble(2);
                            chal.Success = (SuccessInfo)reader.GetInt32(3);
                            chal.EndDate = reader.GetDateTime(4);
                            challenges.Add(chal);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return challenges;
        }
        Challenge DoQuery_OneFinishedChallenge(string query)
        {
            Challenge challenge = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            challenge = new Challenge();
                            challenge.ChallengeID = reader.GetInt32(0);
                            challenge.RecordName = reader.GetString(1);
                            challenge.GoalScore = reader.GetDouble(2);
                            //challenge.Success = (SuccessInfo)reader.GetInt32(3);
                            challenge.EndDate = reader.GetDateTime(4);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return challenge;
        }
        List<Challenge> DoQuery_MultipleFinishedChallenges(string query)
        {
            List<Challenge> challenges = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        challenges = new List<Challenge>();
                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            Challenge chal = new Challenge();
                            chal.ChallengeID = reader.GetInt32(0);
                            chal.RecordName = reader.GetString(1);
                            chal.GoalScore = reader.GetDouble(2);
                            //chal.Success = (SuccessInfo)reader.GetInt32(3);
                            chal.EndDate = reader.GetDateTime(4);
                            challenges.Add(chal);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return challenges;
        }
        List<User> DoQuery_MultipleFriends(string query, int id)
        {
            List<User> friendships = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand comm = new SqlCommand(query, con))
                    {
                        con.Open();

                        friendships = new List<User>();
                        SqlDataReader reader = comm.ExecuteReader();

                        while (reader.Read())
                        {
                            int userid = reader.GetInt32(0);
                            int friendid = reader.GetInt32(1);
                            string username = reader.GetString(2);
                            string friendname = reader.GetString(3);
                            bool pending = reader.GetBoolean(4);

                            User user = new User();
                            if (userid == id)
                            {
                                user.ID = friendid;
                                user.Username = friendname;
                            }
                            else
                            {
                                user.ID = userid;
                                user.Username = username;
                            }
                            friendships.Add(user);
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return friendships;
        }
        public void AddRecordToUser(int userid, Record record)
        {
            string query = String.Format("INSERT INTO RECORDS VALUES({0}, '{1}', '{2}', {3}, {4});", userid, record.Name, record.SelectedImage, (int)record.Success, (int)record.Privacy);
            DoQuery_NoReturn(query);
            query = String.Format("INSERT INTO RECORD_HISTORY VALUES({0}, '{1}', {2}, '{3}');", userid, record.Name, record.BestScore, record.RecordHistory.First().DateAchieved.ToShortDateString());
            DoQuery_NoReturn(query);
            if (record.Goals.Count > 0)
            {
                query = "INSERT INTO GOALS VALUES ";

                //Note: may not account for nullable dates
                foreach (Goal g in record.Goals)
                {
                    query += String.Format("({0}, '{1}', {2}, '{3}'), ", userid, record.Name, g.GoalScore, g.EndDate.ToShortDateString());
                }
                query = query.Remove(query.Length - 2); //Sketch way of removing extra comma and space
                DoQuery_NoReturn(query);
            }
        }

        public void RemoveRecordFromUser(int userid, string recordname)
        {
            string query = String.Format("DELETE FROM RECORDS WHERE UserID = {0} AND RecordName = '{1}';", userid, recordname);
            DoQuery_NoReturn(query);
        }

        public void AddGoalToRecord(int userid, string recordname, Goal goal)
        {
            string query = String.Format("INSERT INTO GOALS VALUES ({0}, '{1}', {2}, '{3}');", userid, recordname, goal.GoalScore, goal.EndDate);
            DoQuery_NoReturn(query);
        }

        public void RemoveGoalFromRecord(int userid, string recordname, Goal goal)
        {
            string query = String.Format("DELETE FROM GOALS WHERE UserID={0} AND RecordName='{1}' AND GoalScore={2};", userid, recordname, goal.GoalScore);
            DoQuery_NoReturn(query);
        }

        public void EditRecordName(int userid, string recordname, string newname)
        {
            string query = String.Format("UPDATE RECORDS SET RecordName='{0}' WHERE UserID={1} AND RecordName='{2}';", newname, userid, recordname);
            DoQuery_NoReturn(query);
            query = String.Format("UPDATE RECORD_HISTORY SET RecordName='{0}' WHERE UserID={1} AND RecordName='{2}';", newname, userid, recordname);
            DoQuery_NoReturn(query);
            query = String.Format("UPDATE GOALS SET RecordName='{0}' WHERE UserID={1} AND RecordName='{2}';", newname, userid, recordname);
            DoQuery_NoReturn(query);
        }

        public void EditRecordPrivacy(int userid, string recordname, PrivacySettings newsetting)
        {
            string query = String.Format("UPDATE RECORDS SET Privacy={0} WHERE UserID={1} AND RecordName='{2}';", (int)newsetting, userid, recordname);
            DoQuery_NoReturn(query);
        }

        public void UpdateRecord(int userid, string recordname, double newscore)
        {
            string query = String.Format("INSERT INTO RECORD_HISTORY VALUES ({0}, '{1}', {2}, '{3}');", userid, recordname, newscore, DateTime.Now.ToShortDateString());
            DoQuery_NoReturn(query);
        }
        
        //Should maybe be optimized to use a single queery
        public Record GetRecordFromUser(int userid, string recordname)
        {
            string query = String.Format("SELECT * FROM RECORDS WHERE UserID={0} AND RecordName='{1}';", userid, recordname);
            Record record = DoQuery_OneRecord(query);
            if (record == null)
                return null;

            query = String.Format("SELECT * FROM RECORD_HISTORY WHERE UserID={0} AND RecordName='{1}';", userid, recordname);
            record.RecordHistory = DoQuery_MultipleRecordItems(query);
            query = String.Format("SELECT * FROM GOALS WHERE UserID={0} AND RecordName='{1}';", userid, recordname);
            record.Goals = DoQuery_MultipleGoals(query);
            return record;
        }

        public List<Record> GetAllUserRecords(int userid)
        {
            string query = String.Format("SELECT * FROM RECORDS WHERE UserID={0};", userid);
            return DoQuery_MultipleRecords(query);
        }
        public List<Record> GetPublicUserRecords(int userid)
        {
            string query = String.Format("SELECT * FROM RECORDS WHERE UserID={0} AND Privacy=0;", userid);
            return DoQuery_MultipleRecords(query);
        }
        public List<Record> GetNonPrivateUserRecords(int userid)
        {
            string query = String.Format("SELECT * FROM RECORDS WHERE UserID={0} AND (Privacy=0 OR Privacy=2);", userid);
            return DoQuery_MultipleRecords(query);
        }

        public void RemoveMultipleGoalsFromRecord(int userid, string recordname, List<Goal> goals)
        {
            string query = String.Format("DELETE FROM GOALS WHERE UserID={0} AND RecordName='{1}' AND (", userid, recordname);
            foreach (Goal g in goals)
            {
                query += String.Format("(GoalScore={0} AND EndDate='{1}') OR ", g.GoalScore, g.EndDate.ToShortDateString());
            }
            query = query.Remove(query.Length - 4); //Remove the OR and two spaces
            query += ");";

            DoQuery_NoReturn(query);
        }
        public void SendNotification(DBNotification notif)
        {
            string chalid = (notif.ChallengeID.HasValue ? notif.ChallengeID.ToString() : "NULL");
            string query = String.Format("INSERT INTO NOTIFICATIONS VALUES ({0}, {1}, {2}, {3});", notif.SenderID, notif.ReceiverID, (int)notif.NotificationType, chalid);
            DoQuery_NoReturn(query);
        }
        public List<DBNotification> GetNotifications(int userid)
        {
            string query = String.Format("SELECT * FROM NOTIFICATIONS WHERE ReceiverID={0};", userid);
            return DoQuery_MultipleNotifications(query);
        }
        public void RemoveNotification(DBNotification notification)
        {
            string chal = "";
            if (notification.ChallengeID != null)
                chal = "=" + notification.ChallengeID;
            else
                chal = " IS NULL";

            string query = String.Format("DELETE FROM NOTIFICATIONS WHERE SenderID={0} AND ReceiverID={1} AND MessageType={2} AND ChallengeID{3}", notification.SenderID, notification.ReceiverID, (int)notification.NotificationType, chal);
            DoQuery_NoReturn(query);
        }
        public List<Challenge> GetUserChallenges(int userid)
        {
            string query = String.Format("SELECT * FROM CHALLENGES WHERE {0} IN (SELECT ParticipantID FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID=CHALLENGES.ChallengeID AND Pending=0);", userid);
            return DoQuery_MultipleChallenges(query);
        }
        public List<Challenge> GetUserChallengesForRecord(int userid, string recordname)
        {
            string query = String.Format("SELECT * FROM CHALLENGES WHERE RecordName='{1}' AND ({0} IN (SELECT ParticipantID FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID=CHALLENGES.ChallengeID AND Pending=0));", userid, recordname);
            return DoQuery_MultipleChallenges(query);
        }
        public Challenge GetChallenge(int chalid)
        {
            string query = String.Format("SELECT * FROM CHALLENGES WHERE ChallengeID={0};", chalid);
            Challenge chal = DoQuery_OneChallenge(query);

            query = String.Format("SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0} AND Pending=0;", chalid);
            chal.Participants = DoQuery_MultipleUsers(query);

            return chal;
        }
        public List<User> GetUserFriends(int userid)
        {
            string query = String.Format("SELECT * FROM FRIENDS WHERE (UserID={0} OR FriendID={0}) AND Pending=0;", userid);
            return DoQuery_MultipleFriends(query, userid);
        }
        public List<User> GetUserFriendRequests(int userid)
        {
            string query = String.Format("SELECT * FROM FRIENDS WHERE Pending=1 AND FriendID={0};", userid);
            return DoQuery_MultipleFriends(query, userid);
        }
        public bool CheckForFriendship(int user1, int user2)
        {
            List<User> friends = GetUserFriends(user1);
            if (friends == null)
                return false;
            else
                return friends.Find(x => x.ID == user2) != null;
        }
        public int CreateChallenge(Challenge challenge)
        {
            string query = String.Format("INSERT INTO CHALLENGES(RecordName, GoalScore, SuccessInfo, EndDate, InProgress) VALUES('{0}', {1}, {2}, '{3}', 0); SELECT * FROM CHALLENGES WHERE ChallengeID=SCOPE_IDENTITY();", challenge.RecordName, challenge.GoalScore, (int)challenge.Success, challenge.EndDate.ToShortDateString());
            int id = DoQuery_OneChallenge(query).ChallengeID;

            query = String.Format("INSERT INTO CHALLENGE_PARTICIPANTS VALUES ");
            foreach (User user in challenge.Participants)
            {
                query += String.Format("({0}, {1}, '{2}', {3}), ", id, user.ID, user.Username, (user.ID == AzureDBAccess.ID ? 0 : 1));
            }
            query = query.Remove(query.Length - 2);
            DoQuery_NoReturn(query);

            return id;
        }
        //Combine these into one query once you confirm they work
        public void WinChallenge(int userid, Challenge challenge)
        {
            string query = String.Format("INSERT INTO FINISHED_CHALLENGES VALUES({0}, '{1}', {2}, {3}, '{4}'); SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0};", challenge.ChallengeID, challenge.RecordName, challenge.GoalScore, userid, DateTime.Now.ToShortDateString());
            //User user = DoQuery_OneUser(query);
            List<User> Participants = DoQuery_MultipleUsers(query);

            //Insert each participant into the finished participants table
            string values = "";
            foreach (User part in Participants)
            {
                values += String.Format("({0}, {1}, '{2}'), ", challenge.ChallengeID, part.ID, part.Username);
            }

            values = values.Remove(values.Length - 2);
            query = String.Format("INSERT INTO FINISHED_CHALLENGE_PARTICIPANTS VALUES {0};", values);
            DoQuery_NoReturn(query);

            query = String.Format("DELETE FROM CHALLENGES WHERE ChallengeID={0};", challenge.ChallengeID);
            DoQuery_NoReturn(query);

            query = String.Format("DELETE FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0};", challenge.ChallengeID);
            DoQuery_NoReturn(query);
        }
        public void ForfeitChallenge(int userid, Challenge challenge)
        {
            //Remove user from CHALLENGE_PARTICIPANTS and get the remaining participants
            string query = String.Format("DELETE FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0} AND ParticipantID={1}; SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0}", challenge.ChallengeID, userid);
            List<User> parts = DoQuery_MultipleUsers(query);

            //If CHALLENGE_PARTICIPANTS now only has one user for this record then end the challenge
            if (parts.Count == 1)
            {
                WinChallenge(parts.First().ID, challenge);
            }
        }
        public void SendFriendRequest(int userid, int friendid)
        {
            string query = String.Format("SELECT * FROM USERS WHERE ID={0} OR ID={1};", userid, friendid); //Should have exactly two results
            List<User> users = DoQuery_MultipleUsers(query);

            string username = users.Find(x => x.ID == userid).Username;
            string friendname = users.Find(x => x.ID == friendid).Username;
            query = String.Format("INSERT INTO FRIENDS VALUES({0}, {1}, '{2}', '{3}', 1);", userid, friendid, username, friendname);
            DoQuery_NoReturn(query);

            DBNotification notif = new DBNotification(userid, friendid, NotificationType.FRIEND_REQUEST);
            SendNotification(notif);
        }
        public void AcceptFriendRequest(int userid, int friendid)
        {
            string query = String.Format("UPDATE FRIENDS SET Pending=0 WHERE (UserID={0} AND FriendID={1}) OR (UserID={1} AND FriendID={0});", userid, friendid);
            DoQuery_NoReturn(query);
        }
        public void DeclineFriendRequest(DBNotification notification)
        {
            string query = String.Format("DELETE FROM FRIENDS WHERE (UserID={0} AND FriendID={1}) OR (UserID={1} AND FriendID={0});", notification.SenderID, notification.ReceiverID);
            DoQuery_NoReturn(query);
        }
        public void RemoveFriend(int user1, int user2)
        {
            string query = String.Format("DELETE FROM FRIENDS WHERE (UserID={0} AND FriendID={1}) OR (UserID={1} AND FriendID={0});", user1, user2);
            DoQuery_NoReturn(query);
        }
        public void AcceptChallengeRequest(DBNotification notification)
        {
            string query = String.Format("UPDATE CHALLENGE_PARTICIPANTS SET Pending=0 WHERE ChallengeID={0} AND ParticipantID={1};", notification.ChallengeID, notification.ReceiverID);
            DoQuery_NoReturn(query);

            //Check if there are any remaining pending participants
            query = String.Format("SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0} AND Pending=1;", notification.ChallengeID);
            List<User> users = DoQuery_MultipleUsers(query);

            //If no then set the challenge to be in progress
            if (users.Count == 0)
            {
                query = String.Format("UPDATE CHALLENGES SET InProgress=1 WHERE ChallengeID={0};", notification.ChallengeID);
                DoQuery_NoReturn(query);
            }
        }
        public void DeclineChallengeRequest(DBNotification notification)
        {
            string query = String.Format("DELETE FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0} AND ParticipantID={1}; SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0};", notification.ChallengeID, notification.ReceiverID);
            int participants = DoQuery_MultipleUsers(query).Count;

            //If there is exactly one participant left (the creator) the challenge should be cancelled
            if (participants == 1)
            {
                query = String.Format("DELETE FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0}; DELETE FROM CHALLENGES WHERE ChallengeID={0};", notification.ChallengeID);
                DoQuery_NoReturn(query);
            }
            else //Otherwise check if there is anyone else pending
            {
                query = String.Format("SELECT ParticipantID, ParticipantName FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID={0} AND Pending=1;", notification.ChallengeID);
                int pendingparticipants = DoQuery_MultipleUsers(query).Count;

                if (pendingparticipants == 0) //If nobody is pending then begin the challenge
                {
                    query = String.Format("UPDATE CHALLENGES SET InProgress=1 WHERE ChallengeID={0};", notification.ChallengeID);
                    DoQuery_NoReturn(query);
                }
            }
        }
        public void AddUser(User user)
        {
            string query = String.Format("INSERT INTO USERS(Username, PasswordHash) VALUES('{0}', '{1}');", user.Username, user.PasswordHash);
            DoQuery_NoReturn(query);
        }
        public User GetUser(int userid)
        {
            string query = String.Format("SELECT * FROM USERS WHERE ID={0};", userid);
            return DoQuery_OneUser(query);
        }
        public User GetUser(string username)
        {
            string query = String.Format("SELECT * FROM USERS WHERE Username='{0}';", username);
            return DoQuery_OneUser(query);
        }
        public List<Challenge> GetFinishedChallenges(int userid)
        {
            string query = String.Format("SELECT * FROM FINISHED_CHALLENGES WHERE {0} IN (SELECT ParticipantID FROM FINISHED_CHALLENGE_PARTICIPANTS WHERE ChallengeID=FINISHED_CHALLENGES.ChallengeID);", userid);
            return DoQuery_MultipleFinishedChallenges(query);
        }
        public Challenge GetFinishedChallenge(int challid)
        {
            string query = String.Format("SELECT * FROM FINISHED_CHALLENGES WHERE ChallengeID={0};", challid);
            Challenge chal = DoQuery_OneFinishedChallenge(query);

            query = String.Format("SELECT ParticipantID, ParticipantName FROM FINISHED_CHALLENGE_PARTICIPANTS WHERE ChallengeID={0};", challid);
            chal.Participants = DoQuery_MultipleUsers(query);

            return chal;
        }
    }
}
