using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace AirplaneParkingAssistant.API.Domain
{
    /// <summary>
    ///  Again DDD value object to encapsulate and help identify the object by its contents so we
    ///  can reuse the object. Im using this value to compare against the area of the airplane.
    /// </summary>
    public class Area : ValueObject
    {
        public static Area Large =  new Area(80, 70);
        public static Area Medium =  new Area(55, 50);
        public static Area Small =  new Area(10, 10);

        public double Length { get; }
        public double Width { get; }
        public double Value => Length * Width;

        public Area(double length, double width)
        {
            Length = length;
            Width = width;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Length;
            yield return Width;
        }

        public override string ToString() => Value.ToString("d");

        // Used to calculate the area including space around on the slot.
        public static Area operator +(Area a1, Area a2) => new Area(a1.Length + a2.Length, a1.Width + a2.Width);
    }
}