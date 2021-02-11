using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public class User : INotifyPropertyChanged
    {
        private int id;
        public int ID 
        { 
            get
            {
                return id;
            }
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        private string username;
        public string Username 
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        public string PasswordHash { get; set; }
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
