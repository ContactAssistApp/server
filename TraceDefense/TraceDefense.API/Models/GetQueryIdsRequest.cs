using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraceDefense.Entities.Geospatial;
using System.ComponentModel.DataAnnotations;

namespace TraceDefense.API.Models
{
    public class GetQueryIdsRequest
    {
        [Required]
        public IList<RegionRef> regions { get; set; }
    }
}
