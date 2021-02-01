using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NewRecord_Backend.Database;
using NewRecord_Backend.Interfaces;
using NewRecord_Backend.Models;
using Timer = System.Timers.Timer;

namespace NewRecord_Backend.ViewModels
{
    public class SocialViewModel : INotifyPropertyChanged
    {
        iDBAccess DBAccess;
        public SocialViewModel()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                Timer timer = new Timer(5000);
                timer.Elapsed += (object source, ElapsedEventArgs e) =>
                {
                    List<DBNotification> notifs = DBAccess.GetNotifications(AzureDBAccess.ID);
                };
                timer.AutoReset = true;
                timer.Enabled = true;
            }).ConfigureAwait(false);
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
