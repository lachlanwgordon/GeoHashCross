using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GeohashCross.Models
{
    public class HashData
    {
        public bool Success { get; set; }
        public String Message { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime Date30W { get; set; }
        public DateTime DJDate { get; set; }
        public bool Use30W { get { return RequestDate != Date30W; } }
        public string DJIA { get; set; }
        public Location Offset { get; set; }
        public Location CurrentLocation { get; set; }
        public Location NearestHashLocation { get; set; }
        public Location GlobalHash { get; set; }
    }
}
