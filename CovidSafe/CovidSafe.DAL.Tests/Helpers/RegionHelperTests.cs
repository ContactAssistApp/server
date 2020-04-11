using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CovidSafe.DAL.Helpers;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.Tests.Helpers
{
    [TestClass]
    public class RegionHelperTests
    {
        void TestRegion(double lat, double lon, int precision, Tuple<double, double> first, Tuple<double, double> second)
        {
            var region = new Region { LatitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
            RegionBoundary rb = RegionHelper.GetRegionBoundary(region);

            Assert.AreEqual(rb.Min.Latitude, first.Item1);
            Assert.AreEqual(rb.Min.Longitude, first.Item2);
            Assert.AreEqual(rb.Max.Latitude, second.Item1);
            Assert.AreEqual(rb.Max.Longitude, second.Item2);
        }

        [TestMethod]
        public void NewYorkRegionTest()
        {
            TestRegion(40.73, -73.93, 0, Tuple.Create(40.0, -74.0), Tuple.Create(41.0, -73.0));
            TestRegion(40.73, -73.93, 1, Tuple.Create(40.5, -74.0), Tuple.Create(41.0, -73.5));
            TestRegion(40.73, -73.93, 2, Tuple.Create(40.5, -74.0), Tuple.Create(40.75, -73.75));
            TestRegion(40.73, -73.93, 3, Tuple.Create(40.625, -74.0), Tuple.Create(40.75, -73.875));
            TestRegion(40.73, -73.93, 4, Tuple.Create(40.6875, -73.9375), Tuple.Create(40.75, -73.875));
            TestRegion(40.73, -73.93, -1, Tuple.Create(40.0, -74.0), Tuple.Create(42.0, -72.0));
            TestRegion(40.73, -73.93, -2, Tuple.Create(40.0, -76.0), Tuple.Create(44.0, -72.0));
            TestRegion(40.73, -73.93, -3, Tuple.Create(40.0, -80.0), Tuple.Create(48.0, -72.0));
            TestRegion(40.73, -73.93, -4, Tuple.Create(32.0, -80.0), Tuple.Create(48.0, -64.0));
        }

        void TestConnectedRegions(Region region, int extension, int precision, IList<Tuple<double, double>> expected)
        {
            List<Region> regions = RegionHelper.GetConnectedRegions(region, extension, precision).ToList();

            Assert.AreEqual(regions.Count, expected.Count);

            for (int i = 0; i < regions.Count; ++i)
            {
                Assert.AreEqual(expected[i].Item1, regions[i].LatitudePrefix);
                Assert.AreEqual(expected[i].Item2, regions[i].LongitudePrefix);
                Assert.AreEqual(precision, regions[i].Precision);
            }
        }

        [TestMethod]
        public void NewYorkConnectedRegionsTest()
        {
            var region = new Region { LatitudePrefix = 40.73, LongitudePrefix = -73.93, Precision = 0 };
            TestConnectedRegions(region, 1, 0, new List<Tuple<double, double>>
            {
                Tuple.Create(39.0, -75.0),
                Tuple.Create(39.0, -74.0),
                Tuple.Create(39.0, -73.0),
                Tuple.Create(40.0, -75.0),
                Tuple.Create(40.0, -74.0),
                Tuple.Create(40.0, -73.0),
                Tuple.Create(41.0, -75.0),
                Tuple.Create(41.0, -74.0),
                Tuple.Create(41.0, -73.0),
            });

            TestConnectedRegions(region, 1, 1, new List<Tuple<double, double>>
            {
                Tuple.Create(39.5, -74.5),
                Tuple.Create(39.5, -74.0),
                Tuple.Create(39.5, -73.5),
                Tuple.Create(39.5, -73.0),
                Tuple.Create(40.0, -74.5),
                Tuple.Create(40.0, -74.0),
                Tuple.Create(40.0, -73.5),
                Tuple.Create(40.0, -73.0),
                Tuple.Create(40.5, -74.5),
                Tuple.Create(40.5, -74.0),
                Tuple.Create(40.5, -73.5),
                Tuple.Create(40.5, -73.0),
                Tuple.Create(41.0, -74.5),
                Tuple.Create(41.0, -74.0),
                Tuple.Create(41.0, -73.5),
                Tuple.Create(41.0, -73.0),
            });

            TestConnectedRegions(region, 1, -1, new List<Tuple<double, double>>
            {
                Tuple.Create(38.0, -76.0),
                Tuple.Create(38.0, -74.0),
                Tuple.Create(38.0, -72.0),
                Tuple.Create(40.0, -76.0),
                Tuple.Create(40.0, -74.0),
                Tuple.Create(40.0, -72.0),
                Tuple.Create(42.0, -76.0),
                Tuple.Create(42.0, -74.0),
                Tuple.Create(42.0, -72.0),
            });
        }

        [TestMethod]
        public void ConnectedRegionsRangeTest()
        {
            var region = new Region { LatitudePrefix = 40.73, LongitudePrefix = -73.93, Precision = 0 };

            {
                var range = RegionHelper.GetConnectedRegionsRange(region, 1, 2);
                Assert.AreEqual(39.75, range.Min.Latitude);
                Assert.AreEqual(-74.25, range.Min.Longitude);
                Assert.AreEqual(41.0, range.Max.Latitude);
                Assert.AreEqual(-73.0, range.Max.Longitude);
            }


            {
                var range = RegionHelper.GetConnectedRegionsRange(region, 1, 0);
                Assert.AreEqual(39.0, range.Min.Latitude);
                Assert.AreEqual(-75.0, range.Min.Longitude);
                Assert.AreEqual(41.0, range.Max.Latitude);
                Assert.AreEqual(-73.0, range.Max.Longitude);
            }

            {
                var range = RegionHelper.GetConnectedRegionsRange(region, 1, -1);
                Assert.AreEqual(39.75, range.Min.Latitude);
                Assert.AreEqual(-74.25, range.Min.Longitude);
                Assert.AreEqual(41.0, range.Max.Latitude);
                Assert.AreEqual(-73.0, range.Max.Longitude);
            }
        }

        [TestMethod]
        public void RegionsCoverageTest()
        {
            var areas = new[] {
                new Area
                {
                    BeginTime = 0,
                    EndTime = 1,
                    RadiusMeters = 100,
                    Location = new Location
                    {
                        Latitude = 40.73,
                        Longitude = -73.93
                    }
                },
                new Area
                {
                    BeginTime = 1,
                    EndTime = 2,
                    RadiusMeters = 100,
                    Location = new Location
                    {
                        Latitude = 40.1,
                        Longitude = -73.93
                    }
                }
            };

            {
                var regions = RegionHelper.GetRegionsCoverage(areas, 0).ToList();
                Assert.AreEqual(1, regions.Count);
            }

            {
                var regions = RegionHelper.GetRegionsCoverage(areas, 1).ToList();
                Assert.AreEqual(2, regions.Count);
            }
        }

    }
}
