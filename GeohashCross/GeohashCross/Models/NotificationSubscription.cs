using System;
using MvvmHelpers;
using Xamarin.Essentials;
using System.Windows.Input;
using SQLite;

namespace GeohashCross.Models
{
    public class NotificationSubscription : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double RadiusInKilometers { get; set; }
        public DateTime AlarmTime { get; set; }//This DateTime is used for time only, the date will be ignored.
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
