using CovidSafe.Entities.Geospatial;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSafe.DAL.Helpers
{
    public static class GeoHelper
    {
        /// <summary>
        /// Get distance in meters between two points
        /// </summary>
        /// <param name="first">Coordinates of the first point</param>
        /// <param name="second">Coordinates of the second point</param>
        /// <returns>Distance between points in meters</returns>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Haversine_formula
        /// reimplementation based on GeoCoordinate.GetDistanceTo System.Device.dll (available only for .NET Framework) 
        /// </remarks>
        public static float DistanceMeters(Coordinates first, Coordinates second)
        {
            double radius = 6376500.0;
            double d1 = first.Latitude * (Math.PI / 180.0);
            double num1 = first.Longitude * (Math.PI / 180.0);
            double d2 = second.Latitude * (Math.PI / 180.0);
            double num2 = second.Longitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return (float)(radius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
    }
}
