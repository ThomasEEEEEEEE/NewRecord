using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using System.Data.SqlClient;

namespace NewRecord_Database
{
    public class AzureDBAccess : iDBAccess
    {
        const string DataSource = "tcp:nr-server.database.windows.net";
        const string UserID = "NRadmin";
        const string Password = "Hootie8667";
        const string InitialCatalog = "NewRecordDB";
        private string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = InitialCatalog;
                return builder.ConnectionString;
                //return "Server=tcp:nr-server.database.windows.net,1433;Initial Catalog=NewRecordDB;Persist Security Info=False;User ID=NRadmin;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            }
        }
        public void DoQuery_NoReturn(string query)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(query, con))
                {
                    con.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }

        public Record DoQuery_OneRecord(string query)
        {
            throw new NotImplementedException();
        }

        public User DoQuery_OneUser(string query)
        {
            throw new NotImplementedException();
        }

        public List<Record> DoQuery_MultipleRecords(string query)
        {
            throw new NotImplementedException();
        }

        public void AddRecordToUser(User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveRecordFromUser(User user)
        {
            throw new NotImplementedException();
        }

        public void AddUser(string username)
        {
            string query = String.Format("INSERT INTO USERS (Username) VALUES ('{0}')", username);
            DoQuery_NoReturn(query);
            //query = String.Format("SELECT ID FROM USERS WHERE Username='{0}'", username);
            //return 
        }
    }
}
