using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace AirplaneParkingAssistant.API.Domain
{
    /// <summary>
    /// Used a DDD concept of a value object for the model of the plane here to 
    /// enforce encapsulation of logic and also provide the airplane enumeration
    /// </summary>
    public class Airplane : ValueObject
    {
        public static Airplane A380 = new Airplane(nameof(A380), ModelType.Jumbo, Area.Large);
        public static Airplane B747 = new Airplane(nameof(B747), ModelType.Jumbo, Area.Large);
        public static Airplane A330 = new Airplane(nameof(A330), ModelType.Jet, Area.Medium);
        public static Airplane B777 = new Airplane(nameof(B777), ModelType.Jet, Area.Medium);
        public static Airplane E195 = new Airplane(nameof(E195), ModelType.Props, Area.Small);
        public static Airplane AC130 = new Airplane(nameof(E195), ModelType.Props, Area.Medium);

        // Consideration - Could leverage reflection here to get the instances via the assembly in case we needed to add more models but forget to include them
        private static readonly List<Airplane> All = new List<Airplane>
        {
            A330, A380, B747,
            B777, E195
        };

        public enum ModelType
        {
            Jumbo,
            Jet,
            Props
        }

        public ModelType Type { get; }
        public string ModelId { get; }
        public Area Size { get; }

        private Airplane(string modelId, ModelType type, Area size)
        {
            ModelId = modelId;
            Type = type;
            Size = size;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ModelId;
            yield return Type;
            yield return Size;
        }

        public static Result<Airplane> FromModelId(string modelId)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                return Result.Failure<Airplane>("Airplane model Id must be provided");

            var trimmedModelId = modelId.Trim().ToUpper();

            var airplane = All.SingleOrDefault(a => a.ModelId == trimmedModelId);
            if (airplane == null)
                return Result.Failure<Airplane>("Airplane model not found");

            return Result.Success(airplane);
        }
    }
}