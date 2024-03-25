using System;
using System.Reflection;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Infrastructure.SQLRepositories.Motorcycle
{
    public class MotorcycleRepository : RepositoryBase<POC.Domain.Models.Entities.Motorcycle>, IMotorcycleRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public MotorcycleRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }


        public async Task<Domain.Models.Entities.Motorcycle> GetMotorcycleByPlate(string plate) =>
            await _dbContext.
                    Connection.
                    QuerySingleOrDefaultAsync<Domain.Models.Entities.Motorcycle>(@"SELECT Id, Identifier, Plate FROM Motorcycle WHERE plate = @plate AND IsDeleted = false",
                                                                new { plate});


        public async Task<IEnumerable<Domain.Models.Entities.Motorcycle>> SearchMotorcycles(string identifier, string plate)
        {

            var sql = @"SELECT id, uniqueid, year, plate, isavailable, modelid, brandid, isdeleted, identifier

                        FROM Motorcycle

                        WHERE isdeleted = false
                        AND plate like @plate" ;


            //A parâmetro de busca 'identificador' é opcional. Opto por refazer a validação no nível da aplicação, ao invéz do processamente do banco.
            //Poderia realizar da seguinte maneira
            //AND (identifier = @identifier OR @identifier IS NULL)
            //Os comandos IS NULL or ISNULL(X, X) são custozos quando utilizados em larga escala.
            if (!String.IsNullOrEmpty(identifier))
                sql += " AND identifier = @identifier";

            return await _dbContext.Connection.QueryAsync<Domain.Models.Entities.Motorcycle>(sql, new { plate = '%' + plate + '%', identifier });


        }

        public async Task UpdateMotorcyclePlate(Domain.Models.Entities.Motorcycle _motorcycle) =>
            await _dBContext.Connection.ExecuteAsync("UPDATE motorcycle SET plate = @plate WHERE id = @id",
                                                    new { plate = _motorcycle.Plate, id = _motorcycle.Id });


        public async Task<MotorcycleRented> GetMotorcycleToRent()
        {

            var _motorcycleToRent = await _dbContext.Connection.QuerySingleOrDefaultAsync<MotorcycleRented>(@"
                                                                SELECT

                                                                Motorcycle.id, 
                                                                Motorcycle.plate, 
                                                                Motorcycle.year,
                                                                MotorcycleBrand.Name as BrandName,
                                                                MotorcycleModel.Name as ModelName


                                                                FROM Motorcycle Motorcycle

                                                                INNER JOIN MotorcycleBrand MotorcycleBrand 
                                                                ON MotorcycleBrand.Id = Motorcycle.BrandId

                                                                INNER JOIN MotorcycleModel MotorcycleModel 
                                                                ON MotorcycleModel.Id = Motorcycle.ModelId

                                                                WHERE IsAvailable = true
                                                                AND Motorcycle.IsDeleted = false

                                                                ORDER BY random()

                                                                LIMIT 1");


            if (_motorcycleToRent != null && _motorcycleToRent.Id > 0)
            {
                //Atualiza moto para indisponível
                await UpdateMotorcycleToUnavailable(_motorcycleToRent.Id);
            }

            return _motorcycleToRent;
        }

        private async Task UpdateMotorcycleToUnavailable(int motorcycleId) =>
            await _dbContext.Connection.ExecuteAsync("update motorcycle SET IsAvailable = false WHERE id = @id", new { id = motorcycleId });

        public async Task UpdateMotorcycleToAvailable(int motorcycleId) =>
           await _dbContext.Connection.ExecuteAsync("update motorcycle SET IsAvailable = true WHERE id = @id", new { id = motorcycleId });

     
    }
}

