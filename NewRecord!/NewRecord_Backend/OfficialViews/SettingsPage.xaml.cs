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
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel vm;
        public SettingsPage()
        {
            InitializeComponent();
            vm = new SettingsViewModel();

            BindingContext = vm;
        }

        private void FRSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            vm.FRchanged();
        }
        private void CHSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            vm.CHchanged();
        }
    }
}