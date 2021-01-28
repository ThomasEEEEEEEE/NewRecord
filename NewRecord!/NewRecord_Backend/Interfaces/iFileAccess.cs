using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;
using System.IO;
using Newtonsoft.Json;

namespace NewRecord_Backend.Interfaces
{
    public interface iFileAccess
    {
        List<Record> GetRecords();
        void WriteRecords(List<Record> records);

        Record GetRecord(string recordname);
        void AddRecord(Record newrecord);
        void RemoveRecord(string recordname);
        void UpdateRecord(string recordname, double newscore);
        void EditRecordName(string oldname, string newname);
        void EditRecordPrivacy(string recordname, PrivacySettings privacy);
        void AddGoalToRecord(string recordname, Goal goal);
        void RemoveGoalFromRecord(string recordname, Goal goal);
        void RemoveGoalsFromRecord(string recordname, List<Goal> goals);
    }
}
