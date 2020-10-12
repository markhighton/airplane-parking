using System.Threading.Tasks;
using AirplaneParkingAssistant.API.Domain.Dtos;
using AirplaneParkingAssistant.API.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AirplaneParkingAssistant.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingZoneController : ControllerBase
    {
        private readonly ILogger<ParkingZoneController> _logger;
        private readonly IMediator _mediator;

        public ParkingZoneController(ILogger<ParkingZoneController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// We return a dto here as we don't want to show our cards to the outside by returning an entity (outside of the domain), this is
        /// generally a bad practice and leads to tight coupling. This acts as our ACL or anti corruption layer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RecommendedSlotDto> GetRecommendedSlot([FromQuery] GetRecommendedSlot.Request request) 
        {
            return await _mediator.Send(request);
        }
    }
}
