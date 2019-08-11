using System;
using System.Collections.Generic;
using System.Linq;
using GeohashCross.Models;
using GeohashCross.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GeohashCross.Views
{
    public partial class OnBoardingView : CarouselView
    {
        public OnBoardingViewModel VM => BindingContext as OnBoardingViewModel;

        public void NextClicked(object sender, EventArgs e)
        {
            var slide = ((sender as View).BindingContext as OnBoardingSlide);
            var index = VM.Slides.IndexOf(slide);

            if (index < VM.Slides.Count() - 1)
            {
                ScrollTo(index + 1);
            }
            else
            {
                VM.DoneComman.Execute(null);
            }


        }

        public void BackClicked(object sender, EventArgs e)
        {
            var slide = ((sender as View).BindingContext as OnBoardingSlide);
            var index = VM.Slides.IndexOf(slide);
            if (index > 0)
            {
                ScrollTo(index - 1);
            }
        }

        public void DoneClicked(object sender, EventArgs e)
        {
            VM.DoneComman.Execute(null);
        }


        public OnBoardingView()
        {
            InitializeComponent();
        }
    }
}
