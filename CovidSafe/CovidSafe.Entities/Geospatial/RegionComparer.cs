using System;
using System.Collections.Generic;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Compares two <see cref="Region"/> objects
    /// </summary>
    public class RegionComparer : IEqualityComparer<Region>
    {
        /// <inheritdoc/>
        public bool Equals(Region x, Region y)
        {
            return x.LatitudePrefix == y.LatitudePrefix
                && x.LongitudePrefix == y.LongitudePrefix
                && x.Precision == y.Precision;
        }

        /// <inheritdoc/>
        public int GetHashCode(Region obj)
        {
            return Tuple.Create(obj.LatitudePrefix, obj.LongitudePrefix, obj.LongitudePrefix)
                .GetHashCode();
        }
    }
}