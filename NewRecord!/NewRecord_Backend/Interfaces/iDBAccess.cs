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
        void DoQuery_NoReturn(string query);
        Record DoQuery_OneRecord(string query);
        List<Record> DoQuery_MultipleRecords(string query);
        User DoQuery_OneUser(string query);

        void AddRecordToUser(User user);
        void RemoveRecordFromUser(User user);
        void AddUser(string Username);
    }
}
