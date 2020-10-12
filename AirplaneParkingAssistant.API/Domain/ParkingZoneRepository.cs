using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace AirplaneParkingAssistant.API.Domain
{
    public interface IParkingZoneRepository
    {
        Task<Result<ParkingZone>> Get();
    }

    /// <summary>
    /// Provide an interface for the data layer so we can change our implementation of it (I would use a microORM like Dapper or full ORM like EFCore)
    /// </summary>
    public class ParkingZoneRepository : IParkingZoneRepository
    {
        public async Task<Result<ParkingZone>> Get()
        {
            // Provide try catch here as we are on the edges of the domain (we don't fully control the data layer from here)
            try
            {
                var fakeData = ParkingZone.Create(25, 50, 25);
                return Result.Success(fakeData);
            }
            catch (Exception e)
            {
                return Result.Failure<ParkingZone>(e.Message);
            }
        }
    }
}