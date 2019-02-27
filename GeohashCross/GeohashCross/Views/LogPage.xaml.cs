using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GeohashCross.Views
{
    public partial class LogPage : ContentPage
    {
        public LogPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.UpdateLogDisplay();
        }
    }
}
