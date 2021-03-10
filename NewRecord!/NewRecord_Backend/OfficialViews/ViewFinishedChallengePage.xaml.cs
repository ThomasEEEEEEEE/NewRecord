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
    public partial class ViewFinishedChallengePage : ContentPage
    {
        ViewFinishedChallengeViewModel vm;
        public ViewFinishedChallengePage(int challid)
        {
            InitializeComponent();
            vm = new ViewFinishedChallengeViewModel(challid);

            BindingContext = vm;
        }
    }
}