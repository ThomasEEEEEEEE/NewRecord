using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;
using NewRecord_Backend.Models;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {
        ListViewModel<Record> vm = new ListViewModel<Record>();
        public RecordPage()
        {
            InitializeComponent();

            Record r = new Record("Soccer");
            r.RecordHistory.Add(new RecordItem(50.5, new DateTime(2018, 3, 13)));
            r.RecordHistory.Add(new RecordItem(109.3, new DateTime(2019, 7, 15)));
            vm.ListView.Add(r);

            r = new Record("Running");
            r.RecordHistory.Add(new RecordItem(8.5, new DateTime(2019, 3, 13)));
            r.RecordHistory.Add(new RecordItem(9.5, new DateTime(2020, 7, 15)));
            vm.ListView.Add(r);

            BindingContext = vm;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            //Go to AddRecordPage
        }
    }
}