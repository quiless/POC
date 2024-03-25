using System;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Entities;

namespace POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle
{
	public interface IMotorcycleModelRepository: IRepository<MotorcycleModel>
    {
		Task<MotorcycleModel> GetMotorcycleModelById(Int32 id);
        Task<MotorcycleModel> GetMotorcycleModelByName(string Name);

    }
}

