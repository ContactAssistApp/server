using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TraceDefense.API.Models
{
    public class GetQueryIdsResponse
    {
        [Required]
        public IList<int> QueryIds { get; set; }
    }
}
