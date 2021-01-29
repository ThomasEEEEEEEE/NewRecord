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
            return record;
        }

        User DoQuery_OneUser(string query)
        {
            User user = null;
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
            return user;
        }

        //For now this does not fill in Goals or RecordHistory
        List<Record> DoQuery_MultipleRecords(string query)
        {
            List<Record> records = null;
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
            return records;
        }

        List<RecordItem> DoQuery_MultipleRecordItems(string query)
        {
            List<RecordItem> recorditems = null;
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

            return recorditems;
        }

        List<Goal> DoQuery_MultipleGoals(string query)
        {
            List<Goal> goals = null;
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
            return goals;
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

        //Purposefully unimplemented
        public List<Goal> GetAllRecordGoals(int userid, string recordname)
        {
            throw new NotImplementedException();
        }

        public List<RecordItem> GetRecordHistory(int userid, string recordname)
        {
            throw new NotImplementedException();
        }
    }
}
