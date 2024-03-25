using System;
using Dapper;
using POC.Artifacts.Helpers;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;

namespace POC.Infrastructure.SQLRepositories.Deliveryman
{
	
    public class DeliverymanRepository : RepositoryBase<Domain.Models.Entities.Deliveryman>, IDeliverymanRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public DeliverymanRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Domain.Models.Entities.Deliveryman> GetDeliverymanByCNPJ(string cnpj) =>
            await _dbContext.
                Connection.
                QuerySingleOrDefaultAsync<Domain.Models.Entities.Deliveryman>(@"SELECT id, name, cnpj, driverlicensenumber FROM deliveryman WHERE cnpj = @cnpj and isdeleted = false",
                                                                            new { cnpj = cnpj.OnlyNumbers() });

        public async Task<Domain.Models.Entities.Deliveryman> GetDeliverymanByDriverLicenseNumber(string driverLicenseNumber) =>
         await _dbContext.
             Connection.
             QuerySingleOrDefaultAsync<Domain.Models.Entities.Deliveryman>(@"SELECT id, name, cnpj, driverlicensenumber FROM deliveryman WHERE driverlicensenumber = @driverlicensenumber and isdeleted = false",
                                                                            new { driverLicenseNumber = driverLicenseNumber.OnlyNumbers() });

        public async Task RefreshDeliverymanDriverLicenseFile(string driverLicenseFileRef, int deliverymanId) =>
            await _dBContext.
                Connection.
                ExecuteAsync("UPDATE deliveryman SET driverlicensefilename = @file WHERE id = @deliverymanId",
                new { deliverymanId, file = driverLicenseFileRef });

        public async Task<Domain.Models.Entities.Deliveryman> GetDeliverymanLoggedByUserId(int userId) =>
            await _dbContext.
                Connection.
                QuerySingleOrDefaultAsync<Domain.Models.Entities.Deliveryman>(@"SELECT hasopenrent, id, name, cnpj, driverlicensenumber, birthdate, driverlicensenumber, driverlicensetypeid, driverlicensefilename, userid FROM deliveryman where userId = @userid",
                                                                               new { userId });

        public async Task UpdateDeliverymanCloseRent(int deliverymanId) =>
            await _dbContext.Connection.ExecuteAsync("update deliveryman SET HasOpenRent = false WHERE id = @id", new { id = deliverymanId });

        public async Task UpdateDeliverymanOpenRent(int deliverymanId) =>
           await _dbContext.Connection.ExecuteAsync("update deliveryman SET HasOpenRent = true WHERE id = @id", new { id = deliverymanId });

        public async Task<IEnumerable<DeliverymanAvailable>> GetDeliverymansAvailableOnDate(DateTime orderDate) =>
            await _dbContext.Connection.QueryAsync<DeliverymanAvailable>(@"SELECT

                                                                                        Deliveryman.Id AS DeliverymanId,
                                                                                        Deliveryman.UniqueId AS DeliverymanUniqueId,
                                                                                        MotorcycleRental.Id AS MotorcycleRentalId

                                                                                        FROM  MotorcycleRental MotorcycleRental

                                                                                        INNER JOIN Deliveryman Deliveryman
                                                                                        ON Deliveryman.Id = MotorcycleRental.DeliverymanId

                                                                                        --Locação não possuí ordem em andamento
                                                                                        WHERE HasopenOrder = false
                                                                                        --Entregador possuí locação em andamento
                                                                                        AND Deliveryman.HasOpenRent = true
                                                                                        --Data da ordem está dentro do período de locação
                                                                                        AND @orderDate BETWEEN MotorcycleRental.StartDate and MotorcycleRental.ExpectedEndDate
                                                                                        --Doublecheck de locação não finalizada
                                                                                        AND MotorcycleRental.WasFinished = false", new { orderDate });

    }
}

