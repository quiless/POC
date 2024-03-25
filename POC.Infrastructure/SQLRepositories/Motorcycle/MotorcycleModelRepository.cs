using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Infrastructure.SQLRepositories.Motorcycle
{
	
    public class MotorcycleModelRepository : RepositoryBase<POC.Domain.Models.Entities.MotorcycleModel>, IMotorcycleModelRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public MotorcycleModelRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }


        public async Task<MotorcycleModel> GetMotorcycleModelById(Int32 id) =>
            await _dbContext.
                    Connection.
                    QuerySingleOrDefaultAsync<MotorcycleModel>(@"SELECT Id, Name FROM MotorcycleModel WHERE Id = @id AND IsDeleted = false",
                                                                new { id });
              

        public async Task<MotorcycleModel> GetMotorcycleModelByName(string Name) =>
               await _dbContext.
                    Connection.
                    QuerySingleOrDefaultAsync<MotorcycleModel>(@"SELECT Id, Name FROM MotorcycleModel WHERE Name = @name AND IsDeleted = false",
                                                                new { Name });


    }
}

