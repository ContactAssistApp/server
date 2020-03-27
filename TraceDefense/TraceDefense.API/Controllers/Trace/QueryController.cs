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
        /// Requests possible <see cref="Query"/> identifiers
        /// </summary>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetQueriesReponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<GetQueriesReponse>> GetAsync(GetQueriesRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs

            // TODO: Submit query
            var result = await this._queryRepo.GetQueriesAsync(request.queryIds, ct);

            GetQueriesReponse results = new GetQueriesReponse { Queries = result };

            return Ok(results);
        }
    }
}