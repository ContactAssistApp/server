using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models;
using TraceDefense.Entities;

namespace TraceDefense.API.Controllers.Trace
{
    /// <summary>
    /// Handles <see cref="TraceEvent"/> query submissions
    /// </summary>
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        /// <summary>
        /// Submits a query for <see cref="TraceEvent"/> objects
        /// </summary>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        /// <response code="404">No query results</response>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QueryResult), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<QueryResult>> PutAsync([FromBody] QueryRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            // JSON library package will make request null if required fields are not met
            if(request == null)
            {
                return BadRequest();
            }

            // TODO: Submit query

            // Process results
            QueryResult results = new QueryResult();

            if(results.Traces.Count() == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }
    }
}