using System;
using NUnit.Framework;
using GeohashCross.Models;

namespace GeohashCross.UnitTests
{
    public class LongitudeTests
    {
        [Test]
        public void IncremementPositiveCentral()
        {
            double lon = 140;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(141, newLon);
        }

        [Test]
        public void IncremementNegativeCentral()
        {
            double lon = -140;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(-141, newLon);
        }

        [Test]
        public void IncremementPositivePrimeMeridian()
        {
            double lon = 0.6;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(1.6, newLon);
        }

        [Test]
        public void IncremementNegativePrimeMeridian()
        {
            double lon = -0.6;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(-1.6, newLon);
        }

        [Test]
        public void IncremementPositiveAntiMeridian()
        {
            double lon = 179.6;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(-179.6, newLon);
        }

        [Test]
        public void IncremementNegativeAntiMeridian()
        {
            double lon = -179.6;
            var newLon = lon.IncrementLongitude();
            Assert.AreEqual(179.6, newLon);
        }

        [Test]
        public void DecrementPositiveCentral()
        {
            double lon = 140;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(139, newLon);
        }

        [Test]
        public void DecrementNegativeCentral()
        {
            double lon = -140;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(-139, newLon);
        }

        [Test]
        public void DecremementPositivePrimeMeridian()
        {
            double lon = 0.6;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(-0.6, newLon);
        }

        [Test]
        public void DecremementNegativePrimeMeridian()
        {
            double lon = -0.6;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(0.6, newLon);
        }

        [Test]
        public void DecremementPositiveAntiMeridian()
        {
            double lon = 179.6;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(178.6, newLon);
        }

        [Test]
        public void DecremementNegativeAntiMeridian()
        {
            double lon = -179.6;
            var newLon = lon.DecrementLongitude();
            Assert.AreEqual(-178.6, newLon);
        }


    }
}
