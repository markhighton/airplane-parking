using System;
using System.Collections.Generic;
using System.Linq;
using AirplaneParkingAssistant.API.Common;

namespace AirplaneParkingAssistant.API.Domain
{
    /// <summary>
    ///  Aggregate route of the domain (the entry point)
    /// </summary>
    public class ParkingZone : Entity
    {
        private readonly List<Slot> _slots;
        // Enforce encapsulation again by using a readonly list
        public IReadOnlyList<Slot> Slots => _slots?.ToList();
        public ParkingZone(List<Slot> slots)
        {
            _slots = slots;
        }

        /// <summary>
        /// Consideration - Could potentially go full functional style programming with this method and create an Extension method e.g. RecommendedSlotFor(A380) to achieve the same result
        /// </summary>
        /// <param name="airplane"></param>
        /// <returns></returns>
        public Slot GetRecommendedSlot(Airplane airplane)
        {
            if (airplane == null)
                throw new ArgumentNullException(nameof(airplane));

            // Consideration - Could probably introduce a rule engine pattern here or specification pattern to help introduce more slot rules but will keep it here for now.
            var recommendedSlot = _slots.FirstOrDefault(slot => slot.IsEmpty && slot.TotalSize.Value >= airplane.Size.Value);
            if (recommendedSlot == null)
            {
                var noSlotsAvailable = new NoSlotsAvailableAlertEvent(Id);
                AddEvent(noSlotsAvailable);
                return Slot.None;
            }

            return recommendedSlot;
        }

        public Slot Park(Airplane airplane)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Factory method to create a parking zone with various sized slots
        /// </summary>
        /// <param name="smallSlots"></param>
        /// <param name="mediumSlots"></param>
        /// <param name="largeSlots"></param>
        /// <returns></returns>
        public static ParkingZone Create(int smallSlots, int mediumSlots, int largeSlots)
        {
            var allSlots = new List<Slot>();
            var slotNumber = 1;
            for (var i = 1; i <= smallSlots; i++)
                allSlots.Add(new Slot(slotNumber++, Area.Small, null, Area.Small, null));

            for (var i = 1; i <= mediumSlots; i++)
                allSlots.Add(new Slot(slotNumber++, Area.Medium, null, Area.Small, null));

            for (var i = 1; i <= largeSlots; i++)
                allSlots.Add(new Slot(slotNumber++, Area.Large, null, Area.Small, null));

            return new ParkingZone(allSlots);
        }
    }

}