﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using System.IO;
using Newtonsoft.Json;

namespace NewRecord_Backend.Database
{
    public class JsonFileAccess : iFileAccess
    {
        private const string FileName = "LocalRecords.json";
        private string FilePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FileName);
            }
        }

        public JsonFileAccess()
        {
            if (!File.Exists(FilePath))
                File.Create(FilePath);
        }
        public List<Record> GetRecords()
        {
            string contents = "";
            try
            {
                contents = File.ReadAllText(FilePath);
            }
            catch (Exception)
            { }
            List<Record> records = JsonConvert.DeserializeObject<List<Record>>(contents);

            if (records == null)
                return new List<Record>();

            return records;
        }
        public void WriteRecords(List<Record> records)
        {
            string newcontents = JsonConvert.SerializeObject(records);
            File.WriteAllText(FilePath, newcontents);
        }
        public void AddRecord(Record newrecord)
        {
            List<Record> records = GetRecords();
            records.Add(newrecord);
            WriteRecords(records);
        }

        public void EditRecordName(string oldname, string newname)
        {
            List<Record> records = GetRecords();
            records.Find(x => x.Name.ToLower() == oldname.ToLower()).Name = newname;
            WriteRecords(records);
        }

        public void EditRecordPrivacy(string recordname, PrivacySettings privacy)
        {
            List<Record> records = GetRecords();
            records.Find(x => x.Name.ToLower() == recordname.ToLower()).Privacy = privacy;
            WriteRecords(records);
        }

        public Record GetRecord(string recordname)
        {
            List<Record> records = GetRecords();
            return records.Find(x => x.Name.ToLower() == recordname.ToLower());
        }

        public void RemoveRecord(string recordname)
        {
            List<Record> records = GetRecords();
            records.RemoveAll(x => x.Name.ToLower() == recordname.ToLower());
            WriteRecords(records);
        }

        public void UpdateRecord(string recordname, double newscore)
        {
            List<Record> records = GetRecords();
            records.Find(x => x.Name.ToLower() == recordname.ToLower()).RecordHistory.Add(new RecordItem(newscore, DateTime.Now));
            WriteRecords(records);
        }

        public void AddGoalToRecord(string recordname, Goal goal)
        {
            List<Record> records = GetRecords();
            records.Find(x => x.Name.ToLower() == recordname.ToLower()).Goals.Add(goal);
            WriteRecords(records);
        }

        public void RemoveGoalFromRecord(string recordname, Goal goal)
        {
            List<Record> records = GetRecords();
            records.Find(x => x.Name.ToLower() == recordname.ToLower()).Goals.Remove(goal);
            WriteRecords(records);
        }
        public void RemoveGoalsFromRecord(string recordname, List<Goal> goals)
        {
            List<Record> records = GetRecords();
            List<Goal> gols = records.Find(x => x.Name.ToLower() == recordname.ToLower()).Goals;
            gols.RemoveAll(x => goals.Contains(x));
            records.Find(x => x.Name.ToLower() == recordname.ToLower()).Goals = gols;
            WriteRecords(records);
        }
    }
}
