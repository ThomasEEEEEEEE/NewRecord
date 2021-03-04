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
    public partial class ViewChallengePage : ContentPage
    {
        ViewChallengeViewModel vm;
        public ViewChallengePage(int challengeid)
        {
            InitializeComponent();
            vm = new ViewChallengeViewModel(challengeid);

            BindingContext = vm;
        }

        private void Forfeit_Clicked(object sender, EventArgs e)
        {
            vm.ForfeitPressed();
        }
    }
}