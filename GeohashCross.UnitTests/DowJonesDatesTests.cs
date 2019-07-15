using System;
using GeohashCross.Services;
using NUnit.Framework;
using Xamarin.Essentials;

namespace Tests
{
    public class DowJonesDatesTests
    {
        [SetUp]
        public void Setup()
        {
        }


        //e.g. Australia
        [Test]
        public void Check30WSouthOfEquatorEastOfGMT()
        {
            var date = new DateTime(2005, 05, 26);
            var locationInVictoria = new Location(-37, 145);
            var expectedDate = date.AddDays(-1);

            var date30w = DowJonesDates.Get30WCompliantDate(date, locationInVictoria);

            Assert.AreEqual(expectedDate, date30w);
        }


        //e.g. Japan
        [Test]
        public void Check30WNorthOfEquatorEastOfGMT()
        {
            var date = new DateTime(2005, 05, 26);
            var location = new Location(35, 136);
            var expectedDate = date.AddDays(-1);

            var date30w = DowJonesDates.Get30WCompliantDate(date, location);

            Assert.AreEqual(expectedDate, date30w);
        }

        //e.g. Portugal
        [Test]
        public void Check30WNorthOfEquatorWestOfGMTEastOf30W()
        {
            var date = new DateTime(2005, 05, 26);
            var location = new Location(39, -8);
            var expectedDate = date.AddDays(-1);

            var date30w = DowJonesDates.Get30WCompliantDate(date, location);

            Assert.AreEqual(expectedDate, date30w);
        }

        //e.g. Inaccessible Island
        [Test]
        public void Check30WSouthOfEquatorWestOfGMTEastOf30W()
        {
            var date = new DateTime(2005, 05, 26);
            var location = new Location(-37, 12);
            var expectedDate = date.AddDays(-1);

            var date30w = DowJonesDates.Get30WCompliantDate(date, location);

            Assert.AreEqual(expectedDate, date30w);
        }

        //e.g. Brazil
        [Test]
        public void Check30WSouthOfEquatorWestOf30W()
        {
            var date = new DateTime(2005, 05, 26);
            var location = new Location(-8, -51);
            var expectedDate = date;

            var date30w = DowJonesDates.Get30WCompliantDate(date, location);

            Assert.AreEqual(expectedDate, date30w);
        }

        //e.g. Canada
        [Test]
        public void Check30WNorthOfEquatorWestOf30W()
        {
            var date = new DateTime(2005, 05, 26);
            var location = new Location(52, -73);
            var expectedDate = date;

            var date30w = DowJonesDates.Get30WCompliantDate(date, location);

            Assert.AreEqual(expectedDate, date30w);
        }



        [Test]
        public void GetDJDateForABusinessMonday()
        {
            var date = new DateTime(2019,07,08);
            var expectedDate = new DateTime(2019, 07, 08);//Open open Monday so should be the same

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForABusinessTuesday()
        {
            var date = new DateTime(2019, 07, 09);
            var expectedDate = new DateTime(2019, 07, 09);//Open open Monday so should be the same

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForABusinessWednesday()
        {
            var date = new DateTime(2019, 07, 10);
            var expectedDate = new DateTime(2019, 07, 10);//Open open weekday so should be the same

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForABusinessThursday()
        {
            var date = new DateTime(2019, 07, 11);
            var expectedDate = new DateTime(2019, 07, 11);//Open open weekday so should be the same

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForABusinessFriday()
        {
            var date = new DateTime(2019, 07, 12);
            var expectedDate = new DateTime(2019, 07, 12);//Open open weekday so should be the same

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForASaturday()
        {
            var date = new DateTime(2019, 07, 13);
            var expectedDate = new DateTime(2019, 07, 12);

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForASunday()
        {
            var date = new DateTime(2019, 07, 14);
            var expectedDate = new DateTime(2019, 07, 12);

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForIndependenceDay2019()
        {
            var date = new DateTime(2019, 07, 4);
            var expectedDate = new DateTime(2019, 07, 3);

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }

        [Test]
        public void GetDJDateForChristmasDay2018()
        {
            var date = new DateTime(2018, 12, 25);
            var expectedDate = new DateTime(2018, 12, 24);

            var djdate = DowJonesDates.GetApplicableDJDate(date);

            Assert.AreEqual(djdate.Data, expectedDate);
        }
    }
}
