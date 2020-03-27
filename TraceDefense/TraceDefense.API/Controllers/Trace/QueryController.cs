using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models;
using TraceDefense.API.Models.Trace;
using TraceDefense.DAL.Repositories;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.API.Controllers.Trace
{
    /// <summary>
    /// Handles trace query submissions
    /// </summary>
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        /// <summary>
        /// <see cref="Query"/> repository
        /// </summary>
        private IQueryRepository _queryRepo;
        /// <summary>
        /// <see cref="RegionRef"/> repository
        /// </summary>
        private IRegionRepository _regionRepo;


        /// <summary>
        /// Creates a new <see cref="QueryController"/> instance
        /// </summary>
        /// <param name="queryRepo"><see cref="Query"/> repository instance</param>
        /// <param name="regionRepo"><see cref="RegionRef"/> repository instance</param>
        public QueryController(IQueryRepository queryRepo, IRegionRepository regionRepo)
        {
            // Assign local values
            this._queryRepo = queryRepo;
            this._regionRepo = regionRepo;
        }

        /// <summary>
        /// Get queries for given query ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /GetQueriesRequest
        ///     {
        ///        "Region": {"id": "39,-74" },
        ///        "LastTimestamp": 0
        ///     }
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <returns>Collection of <see cref="Query"/> objects matching request parameters</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetQueriesReponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<GetQueriesReponse>> PostAsync(GetQueriesRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs

            // TODO: Submit query
            var timestamp = TimestampProvider.GetTimestamp();

            var result = await this._queryRepo.GetQueriesAsync(request.Region, request.LastTimestamp, ct);

            var results = new GetQueriesReponse { Queries = result, Timestamp = timestamp };

            return Ok(results);
        }


        /// <summary>
        /// Publish a query for distribution among devices relevant to Area
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /PublishQueryRequest
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
        public async Task<ActionResult> PutAsync(PublishQueryRequest request)
        {
            CancellationToken ct = new CancellationToken();
            // TODO: Validate inputs
            // TODO:
            var regions = await this._regionRepo.GetRegions(request.Area);
            await this._queryRepo.PublishAsync(regions, request.Query);
            return Ok();
        }
    }
}