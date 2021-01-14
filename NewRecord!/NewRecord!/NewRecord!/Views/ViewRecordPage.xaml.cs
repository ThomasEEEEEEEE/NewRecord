using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.Models;

namespace NewRecord.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecordPage : ContentPage
    {
        public ViewRecordPage(Record record)
        {
            InitializeComponent();

            RecordImage.Source = record.SelectedImage;
            RecordName.Text = record.Name;
            PrivacyInfo.Text = record.Privacy.ToString();
            SuccessSettings.Text = record.Success.ToString();
        }
    }
}