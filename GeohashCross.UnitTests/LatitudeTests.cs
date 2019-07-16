using System;
using NUnit.Framework;
using GeohashCross.Models;

namespace GeohashCross.UnitTests
{
    public class LatitudeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IncrementNorthCentral()
        {
            double latitude = 30;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(31, newLat);
        }

        [Test]
        public void IncrementSouthCentral()
        {
            double latitude = -30;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(-31, newLat);
        }

        [Test]
        public void IncrementNorthEquator()
        {
            double latitude = 0.1;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(1.1, newLat);
        }

        [Test]
        public void IncrementSouthEquator()
        {
            double latitude = -0.1;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(-1.1, newLat);
        }

        [Test]
        public void IncrementNorthPole()
        {
            double latitude = 89.5;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(89.5, newLat);
        }

        [Test]
        public void IncrementSouthPole()
        {
            double latitude = -89.5;
            var newLat = latitude.IncrementLatitude();
            Assert.AreEqual(-89.5, newLat);
        }


        [Test]
        public void DecrementNorthCentral()
        {
            double latitude = 30;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(29, newLat);
        }

        [Test]
        public void DecrementSouthCentral()
        {
            double latitude = -30;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(-29, newLat);
        }

        [Test]
        public void DecrementNorthEquator()
        {
            double latitude = 0.1;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(-0.1, newLat);
        }

        [Test]
        public void DecrementSouthEquator()
        {
            double latitude = -0.1;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(0.1, newLat);
        }

        [Test]
        public void DecrementNorthPole()
        {
            double latitude = 89.5;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(88.5, newLat);
        }

        [Test]
        public void DecrementSouthPole()
        {
            double latitude = -89.5;
            var newLat = latitude.DecrementLatitude();
            Assert.AreEqual(-88.5, newLat);
        }


    }
}
