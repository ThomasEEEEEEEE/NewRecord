using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;
using NewRecord_Backend.Models;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {
        ListViewModel<Record> vm = new ListViewModel<Record>();
        string FileName = "LocalRecords.xml";
        public RecordPage()
        {
            InitializeComponent();

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Record>));
            List<Record> Records = new List<Record>();
            try
            {
                FileStream fs = new FileStream(FilePath + FileName, FileMode.Open);
                Records = (List<Record>)serializer.Deserialize(fs);
            }
            catch (Exception)
            {
                File.Create(FilePath + FileName);
            }

            Records.ForEach(x => vm.ListView.Add(x));

            BindingContext = vm;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddRecordPage());
        }

        private void RListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushModalAsync(new ViewRecordPage(vm.ListView[e.ItemIndex].Name));
        }
    }
}