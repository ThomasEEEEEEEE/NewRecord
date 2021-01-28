using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;
using NewRecord_Backend.Views;
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
            //Records.ListView = new ObservableCollection<Record>(GetRecords());
            //GetRecords().ForEach(x => Records.ListView.Add(x));
            
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
            //Records.ListView = new ObservableCollection<Record>(GetRecords());
            GetRecords().ForEach(x => Records.ListView.Add(x));
            
        }

        public void AddButtonPressed()
        {
            /*var recs = GetRecords();
            foreach (Record r in recs)
            {
                Records.ListView.Add(r);
            }*/
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
