using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;

namespace NewRecord_Backend.OfficialViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicViewRecordPage : ContentPage
    {
        PublicViewRecordViewModel vm;
        public PublicViewRecordPage(int userid, string recordname)
        {
            InitializeComponent();
            vm = new PublicViewRecordViewModel(userid, recordname);

            BindingContext = vm;
        }
    }
}