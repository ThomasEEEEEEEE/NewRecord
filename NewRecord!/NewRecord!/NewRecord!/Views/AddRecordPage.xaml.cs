using NewRecord_Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.Models;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecordPage : ContentPage
    {
        ListViewModel<string> Images;
        string FileName = "LocalRecords.xml";
        public AddRecordPage()
        {
            InitializeComponent();
            Images.ListView.Add("bench-press.png");
            Images.ListView.Add("swimming.png");
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            //Deserialize record data
            XmlSerializer serializer = new XmlSerializer(typeof(List<Record>));
            FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + FileName, FileMode.Open);
            List<Record> records;
            records = (List<Record>)serializer.Deserialize(fs);

            //create new record item with entry info
            Record rec = new Record(NameEntry.Text, Convert.ToDouble(BestScoreEntry.Text));
            rec.SelectedImage = ImageCarousel.CurrentItem.ToString();
            rec.Success = SuccessPicker.SelectedItem.ToString() == "Larger" ? SuccessInfo.LARGER : SuccessInfo.SMALLER;
            if (PrivacyPicker.SelectedItem.ToString() == "Public")
                rec.Privacy = PrivacySettings.PUBLIC;
            else if (PrivacyPicker.SelectedItem.ToString() == "Private")
                rec.Privacy = PrivacySettings.PRIVATE;
            else
                rec.Privacy = PrivacySettings.FRIENDSONLY;

            records.Add(rec);
            //Serialize the new list
            //XmlSerializer serializer = new XmlSerializer(typeof(Record));
            TextWriter writer = new StreamWriter("");
            serializer.Serialize(writer, records);
        }
    }
}