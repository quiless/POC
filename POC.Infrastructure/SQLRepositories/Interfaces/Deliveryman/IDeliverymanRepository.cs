using System;
using MediatR;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Context;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Deliveryman;

namespace POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman
{
	
    public interface IDeliverymanRepository : IRepository<Domain.Models.Entities.Deliveryman>
    {
        Task<Domain.Models.Entities.Deliveryman> GetDeliverymanByCNPJ(string cnpj);
        Task<Domain.Models.Entities.Deliveryman> GetDeliverymanByDriverLicenseNumber(string driverLicenseNumber);
        Task RefreshDeliverymanDriverLicenseFile(string driverLicenseFileRef, int deliverymanId);
        Task<Domain.Models.Entities.Deliveryman> GetDeliverymanLoggedByUserId(int userId);
        Task UpdateDeliverymanCloseRent(int deliverymanId);
        Task UpdateDeliverymanOpenRent(int deliverymanId);
        Task<IEnumerable<DeliverymanAvailable>> GetDeliverymansAvailableOnDate(DateTime orderDate);
    }
}

