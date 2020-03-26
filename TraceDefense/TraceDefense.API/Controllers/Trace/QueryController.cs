using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models;
using TraceDefense.DAL.Repositories;
using TraceDefense.Entities;

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
        /// Time-space discretization manager
        /// </summary>
        private IRegionManager _regionManager;

        /// <summary>
        /// Query repository
        /// </summary>
        private IQueryRepository<int> _queryRepo;


        public QueryController()
        {
            this._regionManager = new TraceDefence.Core.RegionManager();
            this._queryRepo = new TraceDefence.Core.QueryRepository();
        }

        /// <summary>
        /// Submits a query for <see cref="TraceEvent"/> objects
        /// </summary>
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

            var regions = await this._regionManager.GetRegions(request.Area);

            await this._queryRepo.Publish(regions, request.Query);

            return Ok();
        }

        /// <summary>
        /// Submits a query for <see cref="TraceEvent"/> objects
        /// </summary>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        /// <response code="404">No query results</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QueryResult), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<QueryResult>> GetAsync(GetQueriesRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs

            // TODO: Submit query
            var result = await this._queryRepo.GetQueries(request.queryIds);

            QueryResult results = new QueryResult();


            return Ok(results);
        }
    }
}