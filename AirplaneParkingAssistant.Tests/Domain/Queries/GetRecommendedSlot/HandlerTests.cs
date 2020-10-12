using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace AirplaneParkingAssistant.Tests.Domain.Queries.GetRecommendedSlot
{
    using AirplaneParkingAssistant.API.Domain.Queries;
    using API.Domain;
    public class HandlerTests
    {
        private readonly IParkingZoneRepository _parkingZoneRepository;
        private readonly GetRecommendedSlot.Handler _handler;

        public HandlerTests()
        {
            _parkingZoneRepository = A.Fake<IParkingZoneRepository>();
            A.CallTo(() => _parkingZoneRepository.Get())
                .Returns(Result.Success(ParkingZone.Create(25, 50, 25)));

            _handler = new GetRecommendedSlot.Handler(_parkingZoneRepository);
        }

        [Theory]
        [InlineData("A Cat")]
        [InlineData("1")]
        [InlineData("Audi A3")]
        [InlineData("Tin of soup")]
        public async Task Cannot_get_recommended_slot_for_unknown_plane_model(string invalidModelId)
        {
            var request = new GetRecommendedSlot.Request
            {
                AirplaneModelId = invalidModelId
            };

            var result = await _handler.Handle(request, CancellationToken.None);
            result.SlotNumber.Should().Be(-1);
            result.Message.Should().Be("Airplane model not found");
        }

        [Fact]
        public async Task Cannot_get_recommended_slot_without_a_parking_zone()
        {
            A.CallTo(() => _parkingZoneRepository.Get())
                .Returns(Result.Failure<ParkingZone>("Failed to get parking zone"));

            var request = new GetRecommendedSlot.Request
            {
                AirplaneModelId = "A380"
            };

            var result = await _handler.Handle(request, CancellationToken.None);
            result.SlotNumber.Should().Be(-1);
            result.Message.Should().Be("Failed to get parking zone");
        }

        [Fact]
        public async Task Cannot_get_recommended_slot_when_none_are_available()
        {
            A.CallTo(() => _parkingZoneRepository.Get())
                .Returns(Result.Success(ParkingZone.Create(5, 10, 0)));

            var request = new GetRecommendedSlot.Request
            {
                AirplaneModelId = "A380"
            };

            var result = await _handler.Handle(request, CancellationToken.None);
            result.SlotNumber.Should().Be(-1);
            result.Message.Should().Be("No recommended slots - please fly away!!");
        }

        [Fact]
        public async Task Can_get_recommended_slot()
        {
            var request = new GetRecommendedSlot.Request
            {
                AirplaneModelId = "A380"
            };

            var result = await _handler.Handle(request, CancellationToken.None);
            result.SlotNumber.Should().NotBe(-1);
        }
    }
}
