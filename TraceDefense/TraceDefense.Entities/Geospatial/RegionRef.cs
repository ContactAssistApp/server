using Newtonsoft.Json;

namespace TraceDefense.Entities.Geospatial
{
    public class RegionRef
    {
        /// <summary>
        /// Unique <see cref="RegionRef"/> identifier
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int X { get; set; }

        public int Y { get; set; }

        public int T { get; set; }
    }
}
