using System;
using POC.Artifacts.SQL.Repositories;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Entities;

namespace POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle
{
	public interface IMotorcycleBrandRepository: IRepository<MotorcycleBrand>
	{
        Task<MotorcycleBrand> GetMotorcycleBrandById(Int32 id);
        Task<MotorcycleBrand> GetMotorcycleBrandByName(string Name);
    }
}

