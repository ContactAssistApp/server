using CovidSafe.Entities.Geospatial;
using Microsoft.Azure.Cosmos.Spatial;
using System;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="Region"/> implementation which used the GeoSpatial features of 
    /// CosmosDB
    /// </summary>
    public class RegionProperty
    {
        /// <summary>
        /// <see cref="Region"/> coordinates
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Location precision (mantissa)
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// Creates a new <see cref="RegionProperty"/> instance
        /// </summary>
        public RegionProperty()
        {
        }

        /// <summary>
        /// Creates a new <see cref="RegionProperty"/> instance
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        public RegionProperty(Region region)
        {
            this.Location = new Point(region.LongitudePrefix, region.LatitudePrefix);
            this.Precision = region.Precision;
        }
    }
}
