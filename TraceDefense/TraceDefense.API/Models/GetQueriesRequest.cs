﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraceDefense.Entities.Geospatial;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TraceDefense.API.Models
{
    public class GetQueriesRequest
    {
        [Required]
        public IList<int> queryIds { get; set; }
    }
}