﻿using System;
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
    public partial class AddRecordPage : ContentPage
    {
        AddRecordViewModel vm;
        public AddRecordPage()
        {
            InitializeComponent();
            vm = new AddRecordViewModel();
            BindingContext = vm;
        }

        private void AddGoal_Clicked(object sender, EventArgs e)
        {
            vm.AddGoalButtonPressed();
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            vm.AddButtonPressed();
        }
    }
}