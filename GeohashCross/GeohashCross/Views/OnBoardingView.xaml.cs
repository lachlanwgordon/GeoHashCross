using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeohashCross.Models;
using GeohashCross.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GeohashCross.Views
{
    public partial class OnBoardingView : ContentView
    {
        public void Handle_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            Debug.WriteLine($"Scrolled {e.LastVisibleItemIndex}");
        }

        public void Handle_ScrollToRequested(object sender, ScrollToRequestEventArgs e)
        {

            Debug.WriteLine(e.Item);
        }

        public OnBoardingViewModel VM => BindingContext as OnBoardingViewModel;

        public void NextClicked(object sender, EventArgs e)
        {
            var slide = ((sender as View).BindingContext as OnBoardingSlide);
            var index = VM.Slides.IndexOf(slide);

            if (index < VM.Slides.Count() - 1)
            {
                TheCarousel.ScrollTo(index + 1);
            }
            else
            {
                OnDisappearing?.Invoke(this, e);
            }
        }

        public void BackClicked(object sender, EventArgs e)
        {
            var slide = ((sender as View).BindingContext as OnBoardingSlide);
            var index = VM.Slides.IndexOf(slide);
            if (index > 0)
            {
                TheCarousel.ScrollTo(index - 1);
            }
        }

        public void DoneClicked(object sender, EventArgs e)
        {
            OnDisappearing?.Invoke(this, e);
        }


        public OnBoardingView()
        {
            InitializeComponent();
        }

        public delegate void DisappearingHandler(object sender, EventArgs e);
        public event DisappearingHandler OnDisappearing;
    }
}
