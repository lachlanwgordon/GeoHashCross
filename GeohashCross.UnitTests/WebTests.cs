using System;
using System.Threading.Tasks;
using GeohashCross.Services;
using NUnit.Framework;

namespace GeohashCross.UnitTests
{
    public class WebTests
    {
        [SetUp]
        public void Setup()
        {
        }


        /// <summary>
        /// This test is getting data for a valid date so should receive a successful result from the web client
        /// </summary>
        [Test]
        public async Task GetDJIAForLastWeek()
        {
            var djia = await Webclient.GetDjia(DateTime.Today.AddDays(-5), false, false, false);

            Assert.True(djia.Success && !string.IsNullOrWhiteSpace(djia.Data));
        }

        // <summary>
        /// This test is getting data for an invalid date so should receive an unsuccessful result from the web client
        /// </summary>
        [Test]
        public async Task GetDJIAForNextWeek()
        {
            var djia = await Webclient.GetDjia(DateTime.Today.AddDays(5), false, false, false);

            Assert.False(djia.Success || !string.IsNullOrWhiteSpace(djia.Data));
        }
    }
}