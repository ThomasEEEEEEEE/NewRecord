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
        private Type AddPage;
        private Type ViewPage;
        public RecordViewModel(INavigation nav, Type addpage, Type viewpage)
        {
            navigation = nav;
            AddPage = addpage;
            ViewPage = viewpage;

            DBAccess = new AzureDBAccess();
            FileAccess = new JsonFileAccess();
            Records.ListView = new ObservableCollection<Record>(GetRecords());
        }
        ListViewModel<Record> records;
        ListViewModel<Record> Records
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

        private List<Record> GetRecords()
        {
            if (AzureDBAccess.ID == -1)
            {
                return FileAccess.GetRecords();
            }
            else
                return DBAccess.GetAllUserRecords(AzureDBAccess.ID);
        }

        public void OnAppearing()
        {
            Records.ListView.Clear();
            Records.ListView = new ObservableCollection<Record>(GetRecords());
        }

        public void AddButtonPressed()
        {
            navigation.PushModalAsync((Page)Activator.CreateInstance(AddPage));
        }

        public void ItemTapped(int index)
        {
            navigation.PushModalAsync((Page)Activator.CreateInstance(ViewPage));
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
