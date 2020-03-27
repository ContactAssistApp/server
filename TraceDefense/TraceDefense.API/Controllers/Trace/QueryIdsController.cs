using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraceDefense.API.Models;
using TraceDefense.DAL.Repositories;

namespace TraceDefense.API.Controllers.Trace
{
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class QueryIdsController : ControllerBase
    {
        /// <summary>
        /// <see cref="Query"/> repository
        /// </summary>
        private IQueryRepository _queryRepo;

        /// <summary>
        /// Creates a new <see cref="QueryIdsController"/> instance
        /// </summary>
        /// <param name="queryRepo"><see cref="Query"/> repository instance</param>
        public QueryIdsController(IQueryRepository queryRepo)
        {
            // Assign local values
            this._queryRepo = queryRepo;
        }

        /// <summary>
        /// Requests possible <see cref="Query"/> identifiers
        /// </summary>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <returns>Collection of <see cref="Query"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetQueryIdsResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<GetQueryIdsResponse>> GetAsync(GetQueryIdsRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs

            // TODO: Submit query
            var result = await this._queryRepo.GetQueryIdsAsync(request.Regions, ct);

            var results = new GetQueryIdsResponse { QueryIds = result };

            return Ok(results);
        }
    }
}
