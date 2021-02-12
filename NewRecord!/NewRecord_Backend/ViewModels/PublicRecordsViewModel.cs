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

namespace NewRecord_Backend.ViewModels
{
    public class PublicRecordsViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public PublicRecordsViewModel(int userid)
        {
            DBAccess = new AzureDBAccess();

            List<Record> AvailableRecords;
            if (DBAccess.CheckForFriendship(AzureDBAccess.ID, userid))
                AvailableRecords = DBAccess.GetNonPrivateUserRecords(userid);
            else
                AvailableRecords = DBAccess.GetPublicUserRecords(userid);

            ShowRecords = new ListViewModel<Record>(AvailableRecords);
        }

        public void ItemTapped()
        {
            //Unimplemented for now
        }

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
