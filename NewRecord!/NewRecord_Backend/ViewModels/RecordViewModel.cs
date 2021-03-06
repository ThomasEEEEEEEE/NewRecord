﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;
using NewRecord_Backend.OfficialViews;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NewRecord_Backend.ViewModels
{
    public class RecordViewModel : INotifyPropertyChanged
    {
        private iDBAccess DBAccess;
        private iFileAccess FileAccess;
        private INavigation navigation;
        public RecordViewModel(INavigation nav)
        {
            navigation = nav;

            DBAccess = new AzureDBAccess();
            FileAccess = new JsonFileAccess();
            Records = new ListViewModel<Record>();
        }

        private List<Record> GetRecords()
        {
            if (AzureDBAccess.ID == -1)
                return FileAccess.GetRecords();
            else
                return DBAccess.GetAllUserRecords(AzureDBAccess.ID);
        }

        public void OnAppearing()
        {
            Records.ListView.Clear();
            GetRecords().ForEach(x => Records.ListView.Add(x));
            //Records.ListView = new ObservableCollection<Record>(GetRecords());
        }

        public void AddButtonPressed()
        {
            navigation.PushModalAsync(new AddRecordPage());
        }

        public void ItemTapped(int index)
        {
            navigation.PushModalAsync(new ViewRecordPage(Records.ListView[index].Name));
        }

        ListViewModel<Record> records;
        public ListViewModel<Record> Records
        {
            get
            {
                return records;
            }
            set
            {
                records = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Records"));
            }
        }
        #region PropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        void OnPropertyChanged([CallerMemberName] string propertyname = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
