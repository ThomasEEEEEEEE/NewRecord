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
    public partial class RecordPage : ContentPage
    {
        RecordViewModel vm;
        public RecordPage()
        {
            InitializeComponent();
            vm = new RecordViewModel(Navigation);
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            vm.OnAppearing();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            vm.AddButtonPressed();
        }

        private void RListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.ItemTapped(e.ItemIndex);
        }
    }
}