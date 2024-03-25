using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Infrastructure.SQLRepositories.Motorcycle
{

    public class MotorcycleBrandRepository : RepositoryBase<POC.Domain.Models.Entities.MotorcycleBrand>, IMotorcycleBrandRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public MotorcycleBrandRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<MotorcycleBrand> GetMotorcycleBrandById(Int32 id) =>
           await _dbContext.
                   Connection.
                   QuerySingleOrDefaultAsync<MotorcycleBrand>(@"SELECT Id, Name FROM MotorcycleBrand WHERE Id = @id AND IsDeleted = false",
                                                               new { id });


        public async Task<MotorcycleBrand> GetMotorcycleBrandByName(string Name) =>
               await _dbContext.
                    Connection.
                    QuerySingleOrDefaultAsync<MotorcycleBrand>(@"SELECT Id, Name FROM MotorcycleBrand WHERE Name = @name AND IsDeleted = false",
                                                                new { Name });


    }
}

