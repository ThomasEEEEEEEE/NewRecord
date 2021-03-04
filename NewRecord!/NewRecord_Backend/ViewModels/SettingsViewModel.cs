using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NewRecord_Backend.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            FRToggled = Preferences.Get("FRToggled", false);
            CHToggled = Preferences.Get("CHToggled", false);
        }

        public void FRchanged()
        {
            Preferences.Set("FRToggled", FRToggled);
        }

        public void CHchanged()
        {
            Preferences.Set("CHToggled", CHToggled);
        }

        private bool frtoggled;
        private bool chtoggled;
        public bool FRToggled
        {
            get { return frtoggled; }
            set
            {
                frtoggled = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FRToggled"));
            }
        }
        public bool CHToggled
        {
            get { return chtoggled; }
            set
            {
                chtoggled = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CHToggled"));
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
