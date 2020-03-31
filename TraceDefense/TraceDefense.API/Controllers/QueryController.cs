using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models.Protos;
using TraceDefense.API.Models.Trace;
using TraceDefense.DAL.Providers;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Handles <see cref="Query"/> CRUD operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        /// <summary>
        /// <see cref="Query"/> service layer
        /// </summary>
        private IQueryService _queryService;

        /// <summary>
        /// Creates a new <see cref="QueryController"/> instance
        /// </summary>
        /// <param name="queryService"><see cref="Query"/> service layer</param>
        public QueryController(IQueryService queryService)
        {
            // Assign local values
            this._queryService = queryService;
        }

        /// <summary>
        /// Get <see cref="Query"/> objects which match provided unique identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Query?regionId=39%2c-74&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <param name="request"><see cref="QueryRequest"/> parameters</param>
        /// <returns>Collection of <see cref="Query"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<Query>), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IList<Query>>> PostAsync([FromBody] QueryRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(request == null)
            {
                return BadRequest();
            }

            // Get results
            IEnumerable<string> requestedIds = request.RequestedQueries
                .Select(r => r.QueryId);
            IList<Query> result = await this._queryService
                .GetByIdsAsync(requestedIds, ct);

            if(result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Publish a query for distribution among devices relevant to Area
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Query
        ///     {
        ///        "Query": { "tbd": "if you visited Trader Joe's on 3rd Ave 03/26/2020 between 18.00 and 19.00, call me" },
        ///        "Area": { 
        ///             "first": { "lat": 39.5, "lng": -74.5 },
        ///             "second": { "lat": 41.5, "lng": -72.5 }
        ///        }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        /// <response code="404">No query results</response>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(QueryPutRequest request)
        {
            CancellationToken ct = new CancellationToken();
            // TODO: Validate inputs
            var regions = RegionProvider.GetRegions(request.Area);
            await this._queryService.PublishAsync(request.Query, ct);
            return Ok();
        }
    }
}