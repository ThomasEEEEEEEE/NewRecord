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
using Newtonsoft.Json;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {
        ListViewModel<Record> vm = new ListViewModel<Record>();
        string FileName = "LocalRecords.json";
        public RecordPage()
        {
            InitializeComponent();

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            //XmlSerializer serializer = new XmlSerializer(typeof(List<Record>));
            List<Record> Records = new List<Record>();
            try
            {
                string contents = File.ReadAllText(FilePath + FileName);
                Records = JsonConvert.DeserializeObject<List<Record>>(contents);   
            }
            catch (Exception e)
            {
                if (!File.Exists(FilePath + FileName))
                    File.Create(FilePath + FileName);
                else
                    DisplayAlert("Exception", e.Message, "K");
            }

            if (Records != null)
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