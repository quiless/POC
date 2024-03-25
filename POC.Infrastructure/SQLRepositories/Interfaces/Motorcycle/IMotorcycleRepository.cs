using System;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
namespace POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle
{
	public interface IMotorcycleRepository : IRepository<POC.Domain.Models.Entities.Motorcycle>
    {
        Task<Domain.Models.Entities.Motorcycle> GetMotorcycleByPlate(string plate);
        Task<IEnumerable<Domain.Models.Entities.Motorcycle>> SearchMotorcycles(string identifier, string plate);
        Task UpdateMotorcyclePlate(Domain.Models.Entities.Motorcycle _motorcycle);
        Task<MotorcycleRented> GetMotorcycleToRent();
        Task UpdateMotorcycleToAvailable(int motorcycleId);

    }

    
}

