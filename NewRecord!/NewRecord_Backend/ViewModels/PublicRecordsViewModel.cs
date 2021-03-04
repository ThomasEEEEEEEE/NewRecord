using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewRecord_Backend.Models;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Database;
using NewRecord_Backend.OfficialViews;
using Xamarin.Forms;

namespace NewRecord_Backend.ViewModels
{
    public class PublicRecordsViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        INavigation Navigation;
        public PublicRecordsViewModel(int userid, INavigation nav)
        {
            ID = userid;
            Navigation = nav;
            DBAccess = new AzureDBAccess();

            List<Record> AvailableRecords;
            if (DBAccess.CheckForFriendship(AzureDBAccess.ID, userid))
                AvailableRecords = DBAccess.GetNonPrivateUserRecords(userid);
            else
                AvailableRecords = DBAccess.GetPublicUserRecords(userid);

            ShowRecords = new ListViewModel<Record>(AvailableRecords);
        }

        public void ItemTapped(int index)
        {
            _ = Navigation.PushModalAsync(new PublicViewRecordPage(ID, ShowRecords.ListView[index].Name));
        }

        private int ID;
        private ListViewModel<Record> showrecords;
        public ListViewModel<Record> ShowRecords
        {
            get
            {
                return showrecords;
            }
            set
            {
                showrecords = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowRecords"));
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
