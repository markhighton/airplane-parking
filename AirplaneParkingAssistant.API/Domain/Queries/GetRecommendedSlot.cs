using System.Threading;
using System.Threading.Tasks;
using AirplaneParkingAssistant.API.Domain.Dtos;
using MediatR;

namespace AirplaneParkingAssistant.API.Domain.Queries
{
    public class GetRecommendedSlot
    {
        /// <summary>
        /// The request from the API endpoint
        /// </summary>
        public class Request : IRequest<RecommendedSlotDto>
        {
            public string AirplaneModelId { get; set; }
        }

        /// <summary>
        ///  Utilized MeditatR to act as broker between the application layer (controllers) and the domain for loose coupling. This is essentially where we validate and gather our entities / value objects
        /// of the request.
        /// </summary>
        public class Handler : IRequestHandler<Request, RecommendedSlotDto>
        {
            private readonly IParkingZoneRepository _parkingZoneRepository;

            public Handler(IParkingZoneRepository parkingZoneRepository)
            {
                _parkingZoneRepository = parkingZoneRepository;
            }

            public async Task<RecommendedSlotDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var airplaneOrError = Airplane.FromModelId(request.AirplaneModelId);
                if (airplaneOrError.IsFailure)
                    return new RecommendedSlotDto {SlotNumber = -1, Message = airplaneOrError.Error}; // Consideration - Would be better to provide a wrapper here for the dto rather than using a -1 value but will keep it simple for now

                var parkingZoneOrError = await _parkingZoneRepository.Get();
                if (parkingZoneOrError.IsFailure)
                    return new RecommendedSlotDto {SlotNumber = -1, Message = parkingZoneOrError.Error};

                var slot = parkingZoneOrError.Value.GetRecommendedSlot(airplaneOrError.Value);
                if (slot.Number == Slot.None.Number)
                    return new RecommendedSlotDto {SlotNumber = -1, Message = "No recommended slots - please fly away!!" };

                // Consideration - Could use AutoMapper here to map the slot entity to the DTO
                return new RecommendedSlotDto
                {
                    SlotNumber = slot.Number,
                    Message = $"We recommend slot {slot.Number}. This has an area of {slot.TotalSize.Value}m (including space around)"
                };
            }
        }

    }
}
