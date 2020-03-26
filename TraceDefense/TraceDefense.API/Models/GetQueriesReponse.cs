using System.Collections.Generic;
using TraceDefense.Entities;
using System.ComponentModel.DataAnnotations;

namespace TraceDefense.API.Models
{
    /// <summary>
    /// Base query result object
    /// </summary>
    public class GetQueriesReponse
    {
        [Required]
        public IList<Query> Queries { get; set; }
    }
}
