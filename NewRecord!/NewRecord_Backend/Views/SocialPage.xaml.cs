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
    public partial class SocialPage : ContentPage
    {
        SocialViewModel vm;
        public SocialPage()
        {
            InitializeComponent();

            vm = new SocialViewModel();
            BindingContext = vm;
        }
    }
}