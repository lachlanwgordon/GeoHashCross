using System;
using System.Collections.Generic;
using System.Windows.Input;
using GeohashCross.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace GeohashCross.ViewModels
{
    public class OnBoardingViewModel : BaseViewModel
    {
        private bool isVisible = true;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(Slides));
            }
        } 

        public ICommand DoneComman => new Command(Done);

        private void Done(object obj)
        {
            IsVisible = false;
        }

        public IEnumerable<OnBoardingSlide> Slides
        {
            get;
            set;
        } = new List<OnBoardingSlide>
        {
            new OnBoardingSlide{Title = "Welcome to Geohash Cross",
                                Paragraph1 = "Geohashing gives you a destination each day so you can go on an adventure.",
                                //Paragraph2 ="It was created by Randal Munroe in the web comic xkcd.",
                                ImageSource = "home.png",
                                //Paragraph3 = "Each day you'll see a new destination on the map",
                                Paragraph4 = "Go find it and have fun!",
                                NavigationVisible = false


            },
            new OnBoardingSlide{Title = "XKCD",
                                Paragraph1 = "Geohashing gives you a new destination each so you can go on an adventure",
                                Paragraph2 ="It's all based on the xkcd comic from 2008 where Randall Munroe published the algorithm",
                                ImageSource = "geohashing.png",
                                Paragraph3 = "Geohashing gives new destination so you can go on an adventure",
                                Paragraph4 = "Geohashing gives new destination so you can go on an adventure",

            },
            new OnBoardingSlide{Title = "Tabs", Paragraph2="You can change between pages using the tabs at the bottom of the screen"},
            new OnBoardingSlide{Title = "Items", Paragraph2="The first page of the app contains a list of items.\nClick any items for details."},
            new OnBoardingSlide{Title = "Details", Paragraph2="Each item has more information on it's details page, click back when you're done."},
            new OnBoardingSlide{Title = "New Item", Paragraph2="On the list page you can click 'Add' to create another item."},
            new OnBoardingSlide{Title = "About", Paragraph2="The about page has details of the developers of this app"},
            new OnBoardingSlide{Title = "That's all", Paragraph2="Great, you're ready to start using the app!"},
        };

        public OnBoardingViewModel()
        {
        }
    }
}
