using GeohashCross.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;

namespace GeohashCross.Services
{
    class DowJonesDates
    {
        /// <summary>
        /// Checks if dow is open on given date.
        /// If open it returns the date that was input.
        /// If closed it finds most recent day that was open.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Most Recent Date that DJ was open</returns>
        public static Response<DateTime> GetMostRecentDJDate(DateTime date)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            //Dow is closed on weekend
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                Debug.WriteLine($"{date} is a weekend");

                //Check yesterday
                return GetMostRecentDJDate(date.AddDays(-1));//WARNING RECURSION
            }

            //Dow is closed on some public holidays
            if(Holidays.Contains(date))
            {
                Debug.WriteLine($"{date} is a holiday");

                return GetMostRecentDJDate(date.AddDays(-1));//WARNING RECURSION
            }
            Debug.WriteLine($"Using {date}");

            //Check if dow jones data exists, this needs to be updated to use 9:30am new york time
            if(date > DateTime.Today)
            {
                return new Response<DateTime>(date, false, "Dow Jones hasn't openned yet, please check back later");
            }

            return new Response<DateTime>(date, true, "Dow Jones Date is Valid");
        }

        public static DateTime Check30W(DateTime date, Location currentLocation)
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().DeclaringType} {MethodBase.GetCurrentMethod().Name}");

            //Check 30W rule
            var djDate = date;
            if (currentLocation.Longitude >= 30)
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

            //Washington's Birthday
            new DateTime(2018, 2, 19),
            new DateTime(2019, 2, 18),
            new DateTime(2020, 2, 20),

            //Good Friday
            new DateTime(2018, 3, 30),
            new DateTime(2019, 4, 19),
            new DateTime(2020, 4, 10),

            //Memorial Day
            new DateTime(2018, 5, 28),
            new DateTime(2019, 5, 27),
            new DateTime(2020, 5, 25),

            //Independance Day
            new DateTime(2018, 7, 4),
            new DateTime(2019, 7, 4),
            new DateTime(2020, 7, 4),

            //Labor Day
            new DateTime(2018, 9, 3),
            new DateTime(2019, 9, 2),
            new DateTime(2020, 9, 7),

            //Thanksgiving
            new DateTime(2018, 11, 22),
            new DateTime(2019, 11, 28),
            new DateTime(2020, 11, 26),

            //Christmas
            new DateTime(2018, 12, 25),
            new DateTime(2019, 12, 25),
            new DateTime(2020, 12, 25),
            new DateTime(2021, 12, 25),
            new DateTime(2022, 12, 25),
            new DateTime(2023, 12, 25),



        };
    }

}
