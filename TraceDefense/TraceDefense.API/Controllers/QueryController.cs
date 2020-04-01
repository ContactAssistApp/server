using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models.Trace;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Handles <see cref="ProximityQuery"/> CRUD operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> service layer
        /// </summary>
        private IProximityQueryService _queryService;

        /// <summary>
        /// Creates a new <see cref="QueryController"/> instance
        /// </summary>
        /// <param name="queryService"><see cref="ProximityQuery"/> service layer</param>
        public QueryController(IProximityQueryService queryService)
        {
            // Assign local values
            this._queryService = queryService;
        }

        /// <summary>
        /// Get <see cref="ProximityQuery"/> objects matching the provided identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Query
        ///     {
        ///         "RequestedQueries": [{
        ///             "QueryId": "00000000-0000-0000-0000-000000000000",
        ///             "QueryTimestamp": 0
        ///         }]
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <param name="request"><see cref="QueryRequest"/> parameters</param>
        /// <returns>Collection of <see cref="ProximityQuery"/> objects matching request parameters</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<ProximityQuery>), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IList<ProximityQuery>>> PostAsync([FromBody] QueryRequest request)
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
            IEnumerable<ProximityQuery> result = await this._queryService
                .GetByIdsAsync(requestedIds, ct);

            if(result.Count() > 0)
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
            await this._queryService.PublishAsync(request.Query, ct);
            return Ok();
        }
    }
}