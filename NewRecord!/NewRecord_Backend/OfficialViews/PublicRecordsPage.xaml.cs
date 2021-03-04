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
    public partial class PublicRecordsPage : ContentPage
    {
        PublicRecordsViewModel vm;
        public PublicRecordsPage(int id)
        {
            InitializeComponent();
            vm = new PublicRecordsViewModel(id, Navigation);
            BindingContext = vm;
        }
        private void RListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.ItemTapped(e.ItemIndex);
        }
    }
}