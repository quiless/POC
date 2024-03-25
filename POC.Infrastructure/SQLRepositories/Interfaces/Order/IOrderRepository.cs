using System;
using MediatR;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Order;

namespace POC.Infrastructure.SQLRepositories.Interfaces.Order
{
    public interface IOrderRepository : IRepository<Domain.Models.Entities.Order>
    {
        Task<IEnumerable<POC.Domain.Models.Entities.Order>> GetAvailableOrders();
        Task UpdateOrderAccepted(int orderId, int motorcycleRentalId);
        Task UpdateOrderFinished(int orderId);
        Task<IEnumerable<OrdersRegistered>> GetOrders(int? orderStatusId, string CNPJ, DateTime? startDate, DateTime? endDate);
    }
}

