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
        /*void DoQuery_NoReturn(string query);
        Record DoQuery_OneRecord(string query);
        List<Record> DoQuery_MultipleRecords(string query);
        List<RecordItem> DoQuery_MultipleRecordItems(string query);
        List<Goal> DoQuery_MultipleGoals(string query);
        User DoQuery_OneUser(string query);*/

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
        //Below may be redundant
        List<Goal> GetAllRecordGoals(int userid, string recordname);
        List<RecordItem> GetRecordHistory(int userid, string recordname);

    }
}
