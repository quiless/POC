using System;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;

namespace POC.Infrastructure.SQLRepositories.Interfaces.Order
{
	
    public interface IOrderNotificationRepository : IRepository<OrderNotification>
    {

        Task<bool> ValidateOrderNotification(int _motorcycleRentalId, int OrderId);
        Task<IEnumerable<NotificationSent>> GetNotificationsSent(int orderId);
    }
}

