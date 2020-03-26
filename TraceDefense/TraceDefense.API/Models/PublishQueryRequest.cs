using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TraceDefense.Entities;
using TraceDefense.Entities.Search;

namespace TraceDefense.API.Models
{
    public class PublishQueryRequest
    {
        [Required]
        public Area Area { get; set; }

        [Required]
        public Query Query { get; set; }
    }
}
