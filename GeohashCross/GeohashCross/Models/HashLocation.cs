using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GeohashCross.Models
{
    public class HashLocation : Location
    {
        public string Description
        {
            get;
            set;
        }

        public bool IsNeighbour { get; set; }

        public HashLocation()
        {

        }

        public HashLocation(HashData hashData)
        {
            Latitude = hashData.NearestHashLocation.Latitude;
            Longitude = hashData.NearestHashLocation.Longitude;
            Date = hashData.RequestDate;
            Description = hashData.RequestDate == DateTime.Today ? "Today's Hash" : hashData.RequestDate.ToString("yyyy-MM-dd");
        }

        public DateTime Date { get; set; }
        public Position Position
        {
            get
            {
                return new Position(Latitude, Longitude);
            }
        }

        public Color Color
        {
            get
            {
                return DateTime.Today == Date ? Color.Red : Color.Yellow;
            }

        }

        public List<HashLocation> Neighbours
        {
            get
            {


                List<HashLocation> hashes = new List<HashLocation>
                    {
                    new HashLocation{Latitude = Latitude.IncrementLatitude(), Longitude = Longitude.DecrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude.IncrementLatitude(), Longitude = Longitude, Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude.IncrementLatitude(), Longitude = Longitude.IncrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude, Longitude = Longitude.DecrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude, Longitude = Longitude.IncrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude.DecrementLatitude(), Longitude = Longitude.DecrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude.DecrementLatitude(), Longitude = Longitude, Date = Date, Description = Description, IsNeighbour = true },
                    new HashLocation{Latitude = Latitude.DecrementLatitude(), Longitude = Longitude.IncrementLongitude(), Date = Date, Description = Description, IsNeighbour = true },
                    };

                if (Math.Abs(Latitude) >= 89)
                {
                    hashes = hashes.GetRange(3, 5);
                }

                return hashes;
            }
        }


    }
}
