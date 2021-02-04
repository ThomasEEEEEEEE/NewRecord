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

        //For now this does not return goals or record history
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

                        record = new Record();
                        if (reader.Read())
                        {
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
                            if (!reader.IsDBNull(3)) reader.GetInt32(3);

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
            query = "INSERT INTO GOALS VALUES ";

            //Note: may not account for nullable dates
            foreach (Goal g in record.Goals)
            {
                query += String.Format("({0}, '{1}', {2}, '{3}'), ", userid, record.Name, g.GoalScore, g.EndDate.ToShortDateString());
            }
            query = query.Remove(query.Length - 2); //Sketch way of removing extra comma and space
            DoQuery_NoReturn(query);
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

        public void RemoveMultipleGoalsFromRecord(int userid, string recordname, List<Goal> goals)
        {
            string query = String.Format("DELETE FROM GOALS WHERE UserID={0} AND RecordName='{1}' AND (", userid, recordname);
            foreach (Goal g in goals)
            {
                query += String.Format("(GoalScore={0} AND DateAchieved='{1}') OR ", g.GoalScore, g.EndDate.ToShortDateString());
            }
            query = query.Remove(query.Length - 4); //Remove the OR and two spaces
            query += ");";

            DoQuery_NoReturn(query);
        }
        public void SendNotification(DBNotification notif)
        {
            string query = String.Format("INSERT INTO NOTIFICATIONS VALUES ({0}, {1}, {2}, {3});", notif.SenderID, notif.ReceiverID, (int)notif.NotificationType, notif.ChallengeID);
            DoQuery_NoReturn(query);
        }
        public List<DBNotification> GetNotifications(int userid)
        {
            string query = String.Format("SELECT * FROM NOTIFICATIONS WHERE ReceiverID={0};", userid);
            return DoQuery_MultipleNotifications(query);
        }
        public List<Challenge> GetUserChallenges(int userid)
        {
            //SELECT * FROM CHALLENGES WHERE (SELECT ParticipantID FROM CHALLENGE_PARTICIPANTS WHERE ChallengeID=CHALLENGES.ChallengeID)=1;
            //string query = String.Format("SELECT * FROM CHALLENGES JOIN CHALLENGE_PARTICIPANTS AS CP ON ChallengeID WHERE CP.ParticipantID={0};");
            //return DoQuery_MultipleChallenges(query);
            throw new NotImplementedException();
        }
        public List<User> GetUserFriends(int userid)
        {
            string query = String.Format("SELECT * FROM FRIENDS WHERE UserID={0} OR FriendID={0};", userid);
            return DoQuery_MultipleFriends(query, userid);
        }
        public List<User> GetUserFriendRequests(int userid)
        {
            string query = String.Format("SELECT * FROM FRIENDS WHERE Pending=1 AND FriendID={0};", userid);
            return DoQuery_MultipleFriends(query, userid);
        }
        public void CreateChallenge(Challenge challenge)
        {
            string query = String.Format("INSERT INTO CHALLENGES(RecordName, GoalScore, SuccessInfo, EndDate) VALUES('{0}', {1}, {2}, '{3}');", challenge.RecordName, challenge.GoalScore, (int)challenge.Success, challenge.EndDate.ToShortDateString());
            DoQuery_NoReturn(query);
        }
        public void SendFriendRequest(int userid, int friendid)
        {
            string query = String.Format("SELECT * FROM USERS WHERE UserID={0} OR UserID={1};", userid, friendid); //Should have exactly two results
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
            string query = String.Format("UPDATE FRIENDS SET Pending=0 WHERE (UserID={0} AND FriendID={1}) OR (UserID={1} AND Friend{0});", userid, friendid);
            DoQuery_NoReturn(query);
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
    }
}
