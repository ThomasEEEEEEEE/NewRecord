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

        void AddRecordToUser(User user, Record record);
        void AddRecordToUser(int userid, Record record);
        void RemoveRecordFromUser(User user, Record record);
        void RemoveRecordFromUser(int userid, Record record);
        void AddGoalToRecord(User user, Record record, Goal goal);
        void AddGoalToRecord(int userid, Record record, Goal goal);
        void RemoveGoalFromRecord(User user, Record record, Goal goal);
        void RemoveGoalFromRecord(int userid, Record record, Goal goal);
        void EditRecordName(User user, Record record, string newname);
        void EditRecordName(int userid, Record record, string newname);
        void EditRecordPrivacy(User user, Record record, PrivacySettings newsetting);
        void EditRecordPrivacy(int userid, Record record, PrivacySettings newsetting);
        void UpdateRecord(User user, Record record, double newscore);
        void UpdateRecord(int userid, Record record, double newscore);
        Record GetRecordFromUser(User user, string recordname);
        Record GetRecordFromUser(int userid, string recordname);
        List<Record> GetAllUserRecords(User user);
        List<Record> GetAllUserRecords(int userid);
        //Below may be redundant
        List<Goal> GetAllRecordGoals(User user, Record record);
        List<Goal> GetAllRecordGoals(int userid, Record record);
        List<RecordItem> GetRecordHistory(User user, Record record);
        List<RecordItem> GetRecordHistory(int userid, Record record);

    }
}
