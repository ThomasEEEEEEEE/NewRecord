using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NewRecord_Backend.ViewModels;
using NewRecord_Backend.Models;

namespace NewRecord_Backend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddChallengePage : ContentPage
    {
        AddChallengeViewModel vm;
        public AddChallengePage()
        {
            InitializeComponent();
            vm = new AddChallengeViewModel(Navigation);
            BindingContext = vm;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            User user = ((CheckBox)sender).BindingContext as User;
            if (user != null)
                vm.CheckboxChecked(user, e.Value);
        }

        private void CreateButton_Clicked(object sender, EventArgs e)
        {
            vm.CreateButtonClicked();
        }

        private void FListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
        }
    }
}