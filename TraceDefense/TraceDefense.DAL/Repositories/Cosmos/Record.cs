using System;
using System.Collections.Generic;
using System.Text;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    public class Record
    {
        public string Id { get; set; }

        public string Query { get; set; }

        public string RegionId { get; set; }
    }
}
