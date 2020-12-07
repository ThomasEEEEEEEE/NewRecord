using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace NewRecord_Backend.ViewModels
{
    public class ListViewModel<T> : INotifyPropertyChanged
    {
        public ObservableCollection<T> ListView { get; set; }
        public ListViewModel()
        {
            ListView = new ObservableCollection<T>();
        }

        #region PropertyChangedImplementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyname = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
