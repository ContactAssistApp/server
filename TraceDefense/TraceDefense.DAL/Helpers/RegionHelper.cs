using System;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Helpers
{
    public static class RegionHelper
    {
        //TODO: normal implementation. Rough sketch right now
        public static Tuple<Location, Location> ToRange(Region region)
        {
            double delta = Math.Pow(0.1, region.Precision);
            return new Tuple<Location, Location>
            (
                new Location { Lattitude = (float)(region.LattitudePrefix - delta), Longitude = (float)(region.LattitudePrefix - delta) },
                new Location { Lattitude = (float)(region.LattitudePrefix + delta), Longitude = (float)(region.LattitudePrefix + delta) }
            );
        }
    }
}
