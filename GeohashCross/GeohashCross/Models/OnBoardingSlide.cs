using System;
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
    }
}
