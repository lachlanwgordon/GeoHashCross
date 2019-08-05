using GeohashCross.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;
using Location = GeohashCross.Models.Location;

namespace GeohashCross.Services
{
    public class DowJonesDates
    {
        /// <summary>
        /// Checks if dow is open on given date.
        /// If open it returns the date that was input.
        /// If closed it finds most recent day that was open.
        /// e.g. closed on weekends and public holidays so return friday
        /// This Method is not 30w aware and doesn't need to be
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Return the date that should be used for the most recent DJ opening for a given date </returns>
        public static DateTime GetApplicableDJDate(DateTime date)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            //Dow is closed on weekend
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                Debug.WriteLine($"{date} is a weekend");

                //Check yesterday
                return GetApplicableDJDate(date.AddDays(-1));//WARNING RECURSION
            }

            //Dow is closed on some public holidays
            if(Holidays.Contains(date))
            {
                Debug.WriteLine($"{date} is a holiday");

                return GetApplicableDJDate(date.AddDays(-1));//WARNING RECURSION
            }
            Debug.WriteLine($"Using {date}");

            return date;
        }

        /// <summary>
        /// If a location is east of 30w, returns previous date. Otherwise returns same date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static DateTime Get30WCompliantDate(DateTime date, Location location)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            //Check 30W rule
            var djDate = date;
            if (location.Longitude >= -30)
            {
                djDate = date.AddDays(-1);
                Debug.WriteLine($"Using 30W rule. Selected date: {date} DJDate: {djDate}");
            }
            else
            {
                Debug.WriteLine($"Not using 30W rule. Selected date: {date} DJDate: {djDate}");
            }
            return djDate;

        }


        /// <summary>
        /// All public holidays for next three years.
        /// This should move to a server or i'll have to update the app every three years, I'll probably have to do that anyway.
        /// </summary>
        public static List<DateTime> Holidays = new List<DateTime>
        {

            //New Years Day
            new DateTime(2018, 1, 1),
            new DateTime(2019, 1, 1),
            new DateTime(2020, 1, 1),
            new DateTime(2021, 1, 1),
            new DateTime(2022, 1, 1),
            new DateTime(2023, 1, 1),
            new DateTime(2024, 1, 1),
            
            //Martin Luther King Jr Day
            new DateTime(2018, 1, 15),
            new DateTime(2019, 1, 21),
            new DateTime(2020, 1, 20),
            new DateTime(2021, 1, 18),

            //Washington's Birthday
            new DateTime(2018, 2, 19),
            new DateTime(2019, 2, 18),
            new DateTime(2020, 2, 20),
            new DateTime(2021, 2, 15),

            //Good Friday
            new DateTime(2018, 3, 30),
            new DateTime(2019, 4, 19),
            new DateTime(2020, 4, 10),
            new DateTime(2021, 4, 2),

            //Memorial Day
            new DateTime(2018, 5, 28),
            new DateTime(2019, 5, 27),
            new DateTime(2020, 5, 25),
            new DateTime(2021, 5, 31),

            //Independance Day
            new DateTime(2018, 7, 4),
            new DateTime(2019, 7, 4),
            new DateTime(2020, 7, 4),
            new DateTime(2021, 7, 5),

            //Labor Day
            new DateTime(2018, 9, 3),
            new DateTime(2019, 9, 2),
            new DateTime(2020, 9, 7),
            new DateTime(2021, 9, 6),

            //Thanksgiving
            new DateTime(2018, 11, 22),
            new DateTime(2019, 11, 28),
            new DateTime(2020, 11, 26),
            new DateTime(2021, 11, 25),

            //Christmas
            new DateTime(2018, 12, 25),
            new DateTime(2019, 12, 25),
            new DateTime(2020, 12, 25),
            new DateTime(2021, 12, 24),
            new DateTime(2022, 12, 25),
            new DateTime(2023, 12, 25),



        };
    }

}
