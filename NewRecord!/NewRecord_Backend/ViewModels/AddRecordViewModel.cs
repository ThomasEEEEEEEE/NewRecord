using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NewRecord_Backend.Models;

namespace NewRecord_Backend.ViewModels
{
    public class AddRecordViewModel : INotifyPropertyChanged
    {
        public AddRecordViewModel()
        {
            Images = new ListViewModel<string>();
            Goals = new ListViewModel<Goal>();
        }
        private ListViewModel<string> images;
        private ListViewModel<Goal> goals;

        public ListViewModel<string> Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Images"));
            }
        }
        public ListViewModel<Goal> Goals
        {
            get
            {
                return goals;
            }
            set
            {
                goals = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Goals"));
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
