using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Handles <see cref="MatchMessage"/> CRUD operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IMessageService _messageService;

        /// <summary>
        /// Creates a new <see cref="MessageController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public MessageController(IMessageService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Get <see cref="MatchMessage"/> objects matching the provided identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Message
        ///     {
        ///         "RequestedQueries": [{
        ///             "MessageId": "f8bc5992-22b9-491b-a94f-59484c91b705",
        ///             "MessageTimestamp": {
        ///                 "year": 2020,
        ///                 "month": 4,
        ///                 "day": 3,
        ///                 "hour": 4,
        ///                 "minute": 5,
        ///                 "second": 23,
        ///                 "millisecond": 298
        ///             }
        ///         }]
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <param name="request"><see cref="MessageRequest"/> parameters</param>
        /// <returns>Collection of <see cref="MatchMessage"/> objects matching request parameters</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageResponse>> PostAsync([FromBody] MessageRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(request == null || request.RequestedQueries.Count == 0)
            {
                return BadRequest();
            }

            // Get results
            IEnumerable<string> requestedIds = request.RequestedQueries
                   .Select(r => r.MessageId);
            IEnumerable<MatchMessage> result = await this._messageService
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
        /// Publish a <see cref="MatchMessage"/> for distribution among devices relevant to 
        /// <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Message
        ///     {
        ///         "message": {
        ///             "matchProtocolVersion": "1.0.0",
        ///             "areaMatch": [
        ///                 {
        ///                     "userMessage": {
        ///                         "messageData": [{bytestring content}],
        ///                         "signedMessage": [{bytestring content}],
        ///                         "publicKey": [{bytestring content}]
        ///                     }
        ///                     "areas": [
        ///                         {
        ///                             "location": {
        ///                                 "lattitude": -39.1234,
        ///                                 "longitude": 47.1231,
        ///                                 "radiusMeters": 100
        ///                             },
        ///                             "radiusMeters": 250.0,
        ///                             "beginTime": {
        ///                                 "year": 2020,
        ///                                 "month": 3,
        ///                                 "day": 31,
        ///                                 "hour": 12,
        ///                                 "minute": 30,
        ///                                 "second": 12,
        ///                                 "millisecond": 32
        ///                             },
        ///                             "endTime": {
        ///                                 "year": 2020,
        ///                                 "month": 3,
        ///                                 "day": 31,
        ///                                 "hour": 1,
        ///                                 "minute": 49,
        ///                                 "second": 28,
        ///                                 "millisecond": 122
        ///                             }
        ///                         }
        ///                     ],
        ///                     "proximityRadiusMeters": 1000,
        ///                     "durationToleranceSecs": 600
        ///                 }
        ///             ],
        ///             "bluetoothMatch": [],
        ///         },
        ///         "region": {
        ///             "lattitudePrefix": 74.12,
        ///             "longitudePrefix": -39.12,
        ///             "precision": 2
        ///         }
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
        public async Task<ActionResult> PutAsync(AnnounceRequest request)
        {
            CancellationToken ct = new CancellationToken();
            await this._messageService.PublishAsync(request.Region, request.Message, ct);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            CancellationToken ct = new CancellationToken();

            // Seed a message
            MatchMessage message = new MatchMessage();
            AreaMatch areaMatch = new AreaMatch
            {
                DurationToleranceSecs = 600,
                ProximityRadiusMeters = 1000,
                UserMessage = new CryptoMessage
                {
                    MessageData = "Quarantine in place if you visited Trader Joe's here.",
                    PublicKey = "thisisapublickey",
                    SignedMessage = "signedmessage"
                }
            };
            areaMatch.Areas.Add(new Area
            {
                BeginTime = new UTCTime
                {
                    Year = 2020,
                    Month = 3,
                    Day = 31,
                    Hour = 12,
                    Minute = 31,
                    Second = 12
                },
                EndTime = new UTCTime
                {
                    Year = 2020,
                    Month = 3,
                    Day = 31,
                    Hour = 13,
                    Minute = 18,
                    Second = 54
                },
                Location = new Location
                {
                    Lattitude = 74.123456f,
                    Longitude = -39.123456f
                },
                RadiusMeters = 250
            });

            message.AreaMatch.Add(areaMatch);

            AnnounceRequest sample = new AnnounceRequest
            {
                Message = message,
                Region = new Region
                {
                    LattitudePrefix = 74.12345,
                    LongitudePrefix = -39.12345
                }
            };

            await this._messageService.PublishAsync(sample.Region, sample.Message, ct);

            return Ok();
        }
    }
}