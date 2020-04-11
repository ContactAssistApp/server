using CovidSafe.Entities.Protos;
using System;
using System.Collections.Generic;

namespace CovidSafe.DAL.Helpers
{
    public class RegionComparer : IEqualityComparer<Region>
    {
        public bool Equals(Region x, Region y)
        {
            return x.LatitudePrefix == y.LatitudePrefix
                && x.LongitudePrefix == y.LongitudePrefix
                && x.Precision == y.Precision;
        }

        public int GetHashCode(Region obj)
        {
            return Tuple.Create(obj.LatitudePrefix, obj.LongitudePrefix, obj.LongitudePrefix)
                .GetHashCode();
        }
    }
}
