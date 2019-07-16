using System;
using GeohashCross.Services;
using NUnit.Framework;
using Xamarin.Essentials;

namespace GeohashCross.UnitTests
{
    public class HasherTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Sample data from https://xkcd.com/426/
        [Test]
        public void CalculateOffsetCorrect()
        {
            var date = new DateTime(2005, 05, 26);
            var djia = "10458.68";
            var offset = Hasher.CalculateOffset(date, djia);

            var expectedLat = 0.857713;
            var expectedLong = .544544;

            var latitudesMatch = Math.Abs(expectedLat - offset.Latitude) < .00001;
            var longitudesMatch = Math.Abs(expectedLong - offset.Longitude) < .00001;

            Assert.True(latitudesMatch && longitudesMatch);
        }

        //Sample data from https://xkcd.com/426/
        [Test]
        public void CalculateOffsetIncorrect()
        {
            var date = new DateTime(2015, 05, 26);
            var djia = "10458.68";
            var offset = Hasher.CalculateOffset(date, djia);

            var expectedLat = 0.857713;
            var expectedLong = .544544;

            var latitudesDontMatch = Math.Abs(expectedLat - offset.Latitude) > .00001;
            var longitudesDontMatch = Math.Abs(expectedLong - offset.Longitude) > .00001;

            Assert.True(latitudesDontMatch || longitudesDontMatch);
        }


        //E.g. Australia
        [Test]
        public void CalculateHashLocationSouthOfEquatorEastOfGMT()
        {
            var locationInVictoria = new Location(-37, 145);
            var offset = new Location(0.857713, 0.544544);
            var expectedLocation = new Location(-37.857713, 145.544544);

            var calculatedLocation = Hasher.CalculateHashLocation(locationInVictoria, offset);

            var latitudesMatch = Math.Abs(calculatedLocation.Latitude - expectedLocation.Latitude) < .00001;
            var longitudesMatch = Math.Abs(calculatedLocation.Longitude - expectedLocation.Longitude) < .00001;

            Assert.True(latitudesMatch && longitudesMatch);
        }


        //E.g. Japan
        [Test]
        public void CalculateHashLocationNorthOfEquatorEastOfGMT()
        {
            var locationInJapan = new Location(35, 136);
            var offset = new Location(0.857713, 0.544544);
            var expectedLocation = new Location(35.857713, 136.544544);

            var calculatedLocation = Hasher.CalculateHashLocation(locationInJapan, offset);

            var latitudesMatch = Math.Abs(calculatedLocation.Latitude - expectedLocation.Latitude) < .00001;
            var longitudesMatch = Math.Abs(calculatedLocation.Longitude - expectedLocation.Longitude) < .00001;

            Assert.True(latitudesMatch && longitudesMatch);
        }

        //E.g. Portugal
        [Test]
        public void CalculateHashLocationNorthOfEquatorWestOfGMT()
        {
            var locationInPortugal = new Location(39, -8);
            var offset = new Location(0.857713, 0.544544);
            var expectedLocation = new Location(39.857713, -8.544544);

            var calculatedLocation = Hasher.CalculateHashLocation(locationInPortugal, offset);

            var latitudesMatch = Math.Abs(calculatedLocation.Latitude - expectedLocation.Latitude) < .00001;
            var longitudesMatch = Math.Abs(calculatedLocation.Longitude - expectedLocation.Longitude) < .00001;

            Assert.True(latitudesMatch && longitudesMatch);
        }

        //E.g. Brazil
        [Test]
        public void CalculateHashLocationSouthOfEquatorWestOfGMT()
        {
            var locationInBrazil = new Location(-8, -51);
            var offset = new Location(0.857713, 0.544544);
            var expectedLocation = new Location(-8.857713, -51.544544);

            var calculatedLocation = Hasher.CalculateHashLocation(locationInBrazil, offset);

            var latitudesMatch = Math.Abs(calculatedLocation.Latitude - expectedLocation.Latitude) < .00001;
            var longitudesMatch = Math.Abs(calculatedLocation.Longitude - expectedLocation.Longitude) < .00001;

            Assert.True(latitudesMatch && longitudesMatch);
        }

    }
}
