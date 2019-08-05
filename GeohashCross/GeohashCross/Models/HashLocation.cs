using System;
using System.Collections.Generic;
using GeohashCross.Services;

namespace GeohashCross.Models
{
    public class HashLocation : Location
    {
        public bool IsNeighbour { get; set; }
        public bool IsGlobal { get; set; }
        public DateTime Date { get; set; }
        public DateTime DJDate { get; set; }
        public bool Is30WRuleApplied { get; set; }

        public HashLocation(double latitude, double longitude, DateTime date, bool isNeighbour, bool isGlobal) : base(latitude, longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Date = date;
            IsGlobal = isGlobal;
            IsNeighbour = isNeighbour;
        }

        public string Description
        {
            get
            {
                if (IsGlobal)
                {
                    return $"Global Hash for {Date.ToString(Keys.DateFormat)}";
                }
                else
                {
                    return Date == DateTime.Today ? "Today's Hash" : Date.ToString("yyyy-MM-dd");
                }
            }
        }

        public List<HashLocation> Neighbours
        {
            get
            {
                List<HashLocation> hashes = new List<HashLocation>
                    {
                    new HashLocation(Latitude.IncrementLatitude(), Longitude.DecrementLongitude(), Date,true, false ),
                    new HashLocation(Latitude.IncrementLatitude(), Longitude, Date, true, false ),
                    new HashLocation(Latitude.IncrementLatitude(), Longitude.IncrementLongitude(), Date, true, false ),
                    new HashLocation(Latitude, Longitude.DecrementLongitude(), Date, true, false),
                    new HashLocation(Latitude, Longitude.IncrementLongitude(), Date, true, false ),
                    new HashLocation(Latitude.DecrementLatitude(), Longitude.DecrementLongitude(), Date, true, false ),
                    new HashLocation(Latitude.DecrementLatitude(), Longitude, Date, true, false),
                    new HashLocation(Latitude.DecrementLatitude(), Longitude.IncrementLongitude(), Date, true, false),
                    };

                if (Math.Abs(Latitude) >= 89)
                {
                    hashes = hashes.GetRange(3, 5);//Double allocate?
                }

                return hashes;
            }
        }
    }
}
