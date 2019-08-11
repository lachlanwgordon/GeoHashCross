using System;
using GeohashCross.Models;
using NUnit.Framework;

namespace GeohashCross.UnitTests
{
    public class BoundsTest
    {
        [Test]
        public void  CheckEastBothEastOfGMT()
        {
            var min = 3;
            var max = 4;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(max, result.east);
        }

        [Test]
        public void CheckWestBothEastOfGMT()
        {
            var min = 3;
            var max = 4;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(min, result.west);
        }


        [Test]
        public void CheckEastBothWestOfGMT()
        {
            var min = -5;
            var max = -3;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(max, result.east);
        }

        [Test]
        public void CheckWestBothWestOfGMT()
        {
            var min = -5;
            var max = -3;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(min, result.west);
        }



        [Test]
        public void CheckEastEachSideOfGMT()
        {
            var min = -5;
            var max = 5;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(max, result.east);
        }

        [Test]
        public void CheckWestEachSideOfGMT()
        {
            var min = -5;
            var max = 3;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(min, result.west);
        }

        [Test]
        public void CheckEastEachSideOfDateLine()
        {
            var min = -170;
            var max = 170;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(max, result.west);
        }

        [Test]
        public void CheckWestEachSideOfDateLine()
        {
            var min = -170;
            var max = 170;
            var result = LocationExtensions.OrderEastAndWest(min, max);

            Assert.AreEqual(min, result.east);
        }




    }
}
