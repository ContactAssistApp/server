using System;
using CovidSafe.DAL.Helpers;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.Test
{
    [TestClass]
    public class RegionBoundaryTest
    {
        void TestRegion(double lat, double lon, int precision, Tuple<double, double> first, Tuple<double, double> second)
        {
            var region = new Region { LattitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
            RegionBoundary rb = RegionHelper.GetRegionBoundary(region);

            Assert.AreEqual(rb.Min.Lattitude, first.Item1);
            Assert.AreEqual(rb.Min.Longitude, first.Item2);
            Assert.AreEqual(rb.Max.Lattitude, second.Item1);
            Assert.AreEqual(rb.Max.Longitude, second.Item2);
        }

        [TestMethod]
        public void SomeTest()
        {
            TestRegion(40.73, -73.93, 0, Tuple.Create(40.0, -74.0), Tuple.Create(41.0, -73.0));
        }
    }
}
