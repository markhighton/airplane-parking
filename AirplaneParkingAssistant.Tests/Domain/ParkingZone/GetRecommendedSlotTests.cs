using FluentAssertions;
using Xunit;

namespace AirplaneParkingAssistant.Tests.Domain.ParkingZone
{
    using API.Domain;
    public class GetRecommendedSlotTests
    {
        private ParkingZone _parkingZone;
        public GetRecommendedSlotTests()
        {
            _parkingZone = ParkingZone.Create(25, 50, 25);
        }

        [Theory]
        [InlineData("A380")]
        [InlineData("B747")]
        public void Can_find_a_slot_for_a_large_plane(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Large);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }

        [Theory]
        [InlineData("A330")]
        [InlineData("B777")]
        public void Can_find_a_slot_for_a_medium_sized_plane(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Medium);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }


        [Theory]
        [InlineData("E195")]
        public void Can_find_a_slot_for_a_small_sized_plane(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Small);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }

        [Fact]
        public void Small_planes_can_park_in_medium_slots()
        {
            _parkingZone = ParkingZone.Create(0, 1, 0);

            var result = _parkingZone.GetRecommendedSlot(Airplane.E195);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Medium);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }

        [Fact]
        public void Small_planes_can_park_in_large_slots()
        {
            _parkingZone = ParkingZone.Create(0, 0, 1);

            var result = _parkingZone.GetRecommendedSlot(Airplane.E195);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Large);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }

        [Theory]
        [InlineData("A330")]
        [InlineData("B777")]
        public void Medium_sized_planes_can_park_in_large_slots(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            _parkingZone = ParkingZone.Create(0, 0, 1);

            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().NotBeNull();
            result.Size.Should().Be(Area.Large);
            result.SpaceAround.Should().Be(Area.Small);
            result.Occupant.Should().BeNull();
        }

        [Theory]
        [InlineData("A380")]
        [InlineData("B747")]
        public void Large_sized_planes_cannot_park_in_small_slots(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            _parkingZone = ParkingZone.Create(1, 0, 0);

            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().Be(Slot.None);
            _parkingZone.Events.Should().Contain(e => e.GetType() == typeof(NoSlotsAvailableAlertEvent));

        }

        [Theory]
        [InlineData("A330")]
        [InlineData("B777")]
        public void Medium_sized_planes_cannot_park_in_small_slots(string modelId)
        {
            var airplane = Airplane.FromModelId(modelId);
            _parkingZone = ParkingZone.Create(1, 0, 0);

            var result = _parkingZone.GetRecommendedSlot(airplane.Value);
            result.Should().Be(Slot.None);
            _parkingZone.Events.Should().Contain(e => e.GetType() == typeof(NoSlotsAvailableAlertEvent));
        }
    }
}
