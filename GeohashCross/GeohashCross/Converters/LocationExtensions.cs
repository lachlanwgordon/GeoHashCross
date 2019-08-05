using System;

namespace GeohashCross.Models
{
    public static class LocationExtensions
    {

        public static double FixDegreesRange(this double degrees)
        {
            if (degrees < 0)
                return 360 - Math.Abs(degrees);
            if (degrees > 360)
                return degrees - 360;
            return degrees;
        }

        public static double ToDegrees(this double radians)
        {
            var degrees = (180 / Math.PI) * radians;
            return degrees;
        }

        public static double ToRadians(this double degrees)
        {
            var radians = (Math.PI / 180) * degrees;
            return radians;
        }

        /// <summary>
        /// Returns a latitude that is one further away from the equator.
        /// </summary>
        public static double IncrementLatitude(this double originalLatitude)
        {
            double newLat;

            if(Math.Abs(originalLatitude) >= 89)//Very close to a pole
            {
                newLat = originalLatitude;
            }
            else if (originalLatitude >= 0)//North Of equator
            {
                newLat = originalLatitude + 1;
            }
            else//South of equator
            {
                newLat = originalLatitude - 1;
            }
            return newLat;
        }

        /// <summary>
        /// Returns a latitude that is one closer to the equator.
        /// If less than 1 it will cross equator but maintain decimal part. e.g. 0.3 becomes -0.3
        /// </summary>
        public static double DecrementLatitude(this double originalLatitude)
        {

            double newLat;
            if (originalLatitude >= 0)//North Of equator
            {
                if (originalLatitude < 1)//Very close to equator
                {
                    newLat = 0 - originalLatitude;
                }
                else
                {
                    newLat = originalLatitude - 1;
                }
            }
            else//South of equator
            {
                if (originalLatitude > -1)//Very close to equator
                {
                    newLat = 0 - originalLatitude;
                }
                else
                {
                    newLat = originalLatitude + 1;
                }
            }
            return newLat;
        }

        public static double IncrementLongitude(this double originalLongitude)
        {
            double newLon;
            if(Math.Abs(originalLongitude) > 179)
            {
                newLon = 0 - originalLongitude;
            }
            else if(originalLongitude >=0)
            {
                newLon = originalLongitude + 1;

            }
            else
            {
                newLon = originalLongitude - 1;

            }

            return newLon;

        }

        public static double DecrementLongitude(this double originalLongitude)
        {
            double newLon;

            if(Math.Abs(originalLongitude) < 1 )
            {
                newLon = 0 - originalLongitude;
            }

            else if(originalLongitude >= 0)
            {
                newLon = originalLongitude - 1;
            }
            else
            {
                newLon = originalLongitude + 1;
            }




            return newLon;
        }
    }
}
