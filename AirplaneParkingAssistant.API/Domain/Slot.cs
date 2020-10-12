using System;
using AirplaneParkingAssistant.API.Common;

namespace AirplaneParkingAssistant.API.Domain
{
    /// <summary>
    /// Slot entity. This will require persistence to the data layer
    /// </summary>
    public class Slot : Entity
    {
        // Null object pattern. 
        public static Slot None = new Slot(-1, null, null, null ,null);

        public int Number { get; }
        public Area Size { get; }
        public DateTime? OccupiedUntil { get; }

        // Consideration - Added space around the slot to ensure the plane has enough room to park and passengers/luggage can disembark
        public Area SpaceAround { get; }
        public Airplane Occupant { get; }
        public Area TotalSize => Size + SpaceAround;
        public bool IsEmpty => Occupant == null;

        public Slot(int number, Area size, DateTime? occupiedUntil, Area spaceAround, Airplane occupant) 
            // Consideration - A pre-booking integration could be done here by composition at the construction level (e.g. PreBookingTicket). Even a simple flag of IsPreBooked depending on the requirements could suffice
        {
            Number = number;
            Size = size;
            OccupiedUntil = occupiedUntil;
            SpaceAround = spaceAround;
            Occupant = occupant;
        }

        public override string ToString() => Number.ToString();
    }
}