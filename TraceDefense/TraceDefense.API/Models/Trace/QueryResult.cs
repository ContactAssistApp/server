using System.Collections.Generic;

namespace TraceDefense.API.Models.Trace
{
    /// <summary>
    /// Base query result object
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// Collection of <see cref="TraceResult"/> objects matching submitted query
        /// </summary>
        public IEnumerable<TraceResult> Traces { get; set; }
    }
}
