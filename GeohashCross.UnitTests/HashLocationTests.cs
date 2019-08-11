using System;
using System.Collections.Generic;
using GeohashCross.Models;
using NUnit.Framework;

namespace GeohashCross.UnitTests
{
    public class HashLocationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public class MelbourneInteger
        {


            //Melbourne Block
            [Test]
            public void CheckTopLeftNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Latitude, -38);
            }

            [Test]
            public void CheckTopLeftNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Longitude, 144);
            }

            [Test]
            public void CheckTopCenterNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Latitude, -38);
            }

            [Test]
            public void CheckTopCenterNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Longitude, 145);
            }

            [Test]
            public void CheckTopRightNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Latitude, -38);
            }

            [Test]
            public void CheckTopRightNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Longitude, 146);
            }

            [Test]
            public void CheckCentreLeftNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Latitude, -37);
            }

            [Test]
            public void CheckCenterLeftNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Longitude, 144);
            }

            [Test]
            public void CheckCentreRightNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Latitude, -37);
            }

            [Test]
            public void CheckCenterRightNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Longitude, 146);
            }


            [Test]
            public void CheckBottomLeftNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Latitude, -36);
            }

            [Test]
            public void CheckBottomNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Longitude, 144);
            }

            [Test]
            public void CheckBottomCenterNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Latitude, -36);
            }

            [Test]
            public void CheckBottomCenterNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Longitude, 145);
            }

            [Test]
            public void CheckBottomRightNeighbourLatitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Latitude, -36);
            }

            [Test]
            public void CheckBottomRightNeighbourLongitudeInMelbourne()
            {
                var loc = new HashLocation { Latitude = -37, Longitude = 145 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Longitude, 146);
            }
        }







        public class AlgeriaIntegerNorthEast
        {


            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Latitude, 4);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Longitude, 2);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Latitude, 4);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Longitude, 3);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Latitude, 4);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Longitude, 4);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Latitude, 3);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Longitude, 2);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Latitude, 3);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Longitude, 4);
            }


            [Test]
            public void CheckNeighbour5Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Latitude, 2);
            }

            [Test]
            public void CheckNeighbour5Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Longitude, 2);
            }

            [Test]
            public void CheckNeighbour6Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Latitude, 2);
            }

            [Test]
            public void CheckNeighbour6Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Longitude, 3);
            }

            [Test]
            public void CheckNeighbour7Lat()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Latitude, 2);
            }

            [Test]
            public void CheckNeighbour7Lon()
            {
                var loc = new HashLocation { Latitude = 3, Longitude = 3 };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Longitude, 4);
            }
        }


        public class AlgeriaFractionStartNorthEastCrossBoth
        {
            double lat = 0.3;
            double lon = 0.3;

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Longitude, -0.3);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Longitude, 0.3);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Longitude, 1.3);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Longitude, -0.3);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Longitude, 1.3);
            }


            [Test]
            public void CheckNeighbour5Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour5Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Longitude, -0.3);
            }

            [Test]
            public void CheckNeighbour6Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour6Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Longitude, 0.3);
            }

            [Test]
            public void CheckNeighbour7Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour7Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Longitude, 1.3);
            }
        }

        public class EquatorPrimeMeridianStartSouthWestCrossBoth
        {
            double lat = -0.3;
            double lon = -0.3;

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Latitude, -1.3);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Longitude, 0.3);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Latitude, -1.3);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Longitude, -0.3);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Latitude, -1.3);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Longitude, -1.3);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Longitude, 0.3);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Longitude, -1.3);
            }


            [Test]
            public void CheckNeighbour5Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour5Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Longitude, 0.3);
            }

            [Test]
            public void CheckNeighbour6Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour6Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Longitude, -0.3);
            }

            [Test]
            public void CheckNeighbour7Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour7Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Longitude, -1.3);
            }
        }

        public class EquatorAntiMeridianStartSouthWestCrossBoth
        {
            double lat = 0.3;
            double lon = 179.3;

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[0].Longitude, 178.3);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[1].Longitude, 179.3);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Latitude, 1.3);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[2].Longitude, -179.3);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[3].Longitude, 178.3);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Latitude, 0.3);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[4].Longitude, -179.3);
            }


            [Test]
            public void CheckNeighbour5Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour5Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[5].Longitude, 178.3);
            }

            [Test]
            public void CheckNeighbour6Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour6Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[6].Longitude, 179.3);
            }

            [Test]
            public void CheckNeighbour7Lat()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Latitude, -0.3);
            }

            [Test]
            public void CheckNeighbour7Lon()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                var neighbours = loc.Neighbours;

                Assert.AreEqual(neighbours[7].Longitude, -179.3);
            }
        }

        public class NorthPolePrimeMeridian
        {
            double lat = 89.3;
            double lon = 0.3;
            List<HashLocation> Neighbours;

            [SetUp]
            public void Setup()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                Neighbours = loc.Neighbours;
            }

            [Test]
            public void CheckNumberOfNeighbours()
            {
                Assert.AreEqual(5, Neighbours.Count);


            }

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                Assert.AreEqual(89.3, Neighbours[0].Latitude);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                Assert.AreEqual(-0.3, Neighbours[0].Longitude);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                Assert.AreEqual(89.3, Neighbours[1].Latitude);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                Assert.AreEqual(1.3, Neighbours[1].Longitude);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                Assert.AreEqual(88.3, Neighbours[2].Latitude);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                Assert.AreEqual(-0.3, Neighbours[2].Longitude);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                Assert.AreEqual(88.3, Neighbours[3].Latitude);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                Assert.AreEqual(0.3, Neighbours[3].Longitude);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                Assert.AreEqual(88.3, Neighbours[4].Latitude);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                Assert.AreEqual(1.3, Neighbours[4].Longitude);
            }
        }

        public class NorthPoleAntiMeridian
        {
            double lat = 89.3;
            double lon = 179.3;
            List<HashLocation> Neighbours;

            [SetUp]
            public void Setup()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                Neighbours = loc.Neighbours;
            }

            [Test]
            public void CheckNumberOfNeighbours()
            {
                Assert.AreEqual(5, Neighbours.Count);
            }

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                Assert.AreEqual(89.3, Neighbours[0].Latitude);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                Assert.AreEqual(178.3, Neighbours[0].Longitude);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                Assert.AreEqual(89.3, Neighbours[1].Latitude);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                Assert.AreEqual(-179.3, Neighbours[1].Longitude);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                Assert.AreEqual(88.3, Neighbours[2].Latitude);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                Assert.AreEqual(178.3, Neighbours[2].Longitude);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                Assert.AreEqual(88.3, Neighbours[3].Latitude);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                Assert.AreEqual(179.3, Neighbours[3].Longitude);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                Assert.AreEqual(88.3, Neighbours[4].Latitude);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                Assert.AreEqual(-179.3, Neighbours[4].Longitude);
            }
        }

        public class SouthPolePrimeMeridian
        {
            double lat = -89.3;
            double lon = 0.3;
            List<HashLocation> Neighbours;

            [SetUp]
            public void Setup()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                Neighbours = loc.Neighbours;
            }

            [Test]
            public void CheckNumberOfNeighbours()
            {
                Assert.AreEqual(5, Neighbours.Count);


            }

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                Assert.AreEqual(-89.3, Neighbours[0].Latitude);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                Assert.AreEqual(-0.3, Neighbours[0].Longitude);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                Assert.AreEqual(-89.3, Neighbours[1].Latitude);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                Assert.AreEqual(1.3, Neighbours[1].Longitude);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[2].Latitude);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                Assert.AreEqual(-0.3, Neighbours[2].Longitude);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[3].Latitude);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                Assert.AreEqual(0.3, Neighbours[3].Longitude);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[4].Latitude);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                Assert.AreEqual(1.3, Neighbours[4].Longitude);
            }
        }

        public class SouthPoleAntiMeridian
        {
            double lat = -89.3;
            double lon = 179.3;
            List<HashLocation> Neighbours;

            [SetUp]
            public void Setup()
            {
                var loc = new HashLocation { Latitude = lat, Longitude = lon };
                Neighbours = loc.Neighbours;
            }

            [Test]
            public void CheckNumberOfNeighbours()
            {
                Assert.AreEqual(5, Neighbours.Count);
            }

            //Melbourne Block
            [Test]
            public void CheckNeighbour0Lat()
            {
                Assert.AreEqual(-89.3, Neighbours[0].Latitude);
            }

            [Test]
            public void CheckNeighbour0Lon()
            {
                Assert.AreEqual(178.3, Neighbours[0].Longitude);
            }

            [Test]
            public void CheckNeighbour1Lat()
            {
                Assert.AreEqual(-89.3, Neighbours[1].Latitude);
            }

            [Test]
            public void CheckNeighbour1Lon()
            {
                Assert.AreEqual(-179.3, Neighbours[1].Longitude);
            }

            [Test]
            public void CheckNeighbour2Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[2].Latitude);
            }

            [Test]
            public void CheckNeighbour2Lon()
            {
                Assert.AreEqual(178.3, Neighbours[2].Longitude);
            }

            [Test]
            public void CheckNeighbour3Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[3].Latitude);
            }

            [Test]
            public void CheckNeighbour3Lon()
            {
                Assert.AreEqual(179.3, Neighbours[3].Longitude);
            }

            [Test]
            public void CheckNeighbour4Lat()
            {
                Assert.AreEqual(-88.3, Neighbours[4].Latitude);
            }

            [Test]
            public void CheckNeighbour4Lon()
            {
                Assert.AreEqual(-179.3, Neighbours[4].Longitude);
            }
        }


    }
}
