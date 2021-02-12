using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;

namespace NewRecord_Backend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicRecordsPage : ContentPage
    {
        PublicRecordsViewModel vm;
        public PublicRecordsPage(int userid)
        {
            InitializeComponent();
            vm = new PublicRecordsViewModel(userid);

            BindingContext = vm;
        }

        private void RListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.ItemTapped();
        }
    }
}