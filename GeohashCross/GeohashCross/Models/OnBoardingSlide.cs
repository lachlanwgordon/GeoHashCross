using System;
using System.Collections.Generic;

namespace GeohashCross.Models
{
    public class OnBoardingSlide
    {
        public bool NavigationVisible
        {
            get;
            set;
        } = true;



        public string Title { get; set; }
        public string Paragraph1 { get; set; }
        public string ImageSource { get; set; }
        public string Paragraph2 { get; set; }
        public string Paragraph3 { get; internal set; }
        public string Paragraph4 { get; internal set; }
        public int SlideNumber { get; set; }
        public int NumberOfSlides { get; set; }
        public List<bool> SlideDots
        {
            get
            {
                var list = new List<bool>();
                for(int i = 1; i <= NumberOfSlides; i ++)
                {
                    list.Add(SlideNumber == i);
                }
                return list;
            }
        }
    }
}
