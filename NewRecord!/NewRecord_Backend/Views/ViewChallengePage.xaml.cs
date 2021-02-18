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
    public partial class ViewChallengePage : ContentPage
    {
        ViewChallengeViewModel vm;
        public ViewChallengePage(int ChallengeID)
        {
            InitializeComponent();
            vm = new ViewChallengeViewModel(ChallengeID);

            BindingContext = vm;
        }
    }
}