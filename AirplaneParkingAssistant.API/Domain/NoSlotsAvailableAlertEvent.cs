using System;
using AirplaneParkingAssistant.API.Common;

namespace AirplaneParkingAssistant.API.Domain
{
    /// <summary>
    /// Consideration - Can be dispatched to various services via a message bus to notify them
    /// </summary>
    public class NoSlotsAvailableAlertEvent : IEvent
    {
        public Guid ZoneId { get; }

        public NoSlotsAvailableAlertEvent(Guid zoneId)
        {
            ZoneId = zoneId;
        }
    }
}