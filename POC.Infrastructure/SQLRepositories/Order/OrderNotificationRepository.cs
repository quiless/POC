using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces.Order;

namespace POC.Infrastructure.SQLRepositories.Order
{
	
    public class OrderNotificationRepository : RepositoryBase<OrderNotification>, IOrderNotificationRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public OrderNotificationRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<bool> ValidateOrderNotification(int motorcycleRentalId, int orderId) =>
            await _dbContext.
                    Connection.
                    QuerySingleAsync<bool>("SELECT count(1) > 0 from ordernotification where motorcyclerentalid = @motorcycleRentalId and orderid = @orderId and isdeleted = false",
                                            new { orderId, motorcycleRentalId });


        public async Task<IEnumerable<NotificationSent>> GetNotificationsSent(int orderId) =>
            await _dbContext.
                    Connection.
                    QueryAsync<NotificationSent>(@"SELECT 

                                                    ordernotification.Id as NotificationId,
                                                    ordernotification.CreateDate,
                                                    deliveryman.name as DeliverymanName,
                                                    deliveryman.cnpj as DeliverymanCNPJ

                                                    FROM ordernotification ordernotification

                                                    left join motorcyclerental motorcyclerental
                                                    on motorcyclerental.Id = ordernotification.motorcyclerentalid

                                                    left join deliveryman deliveryman
                                                    on deliveryman.Id = motorcyclerental.deliverymanId

                                                    WHERE ordernotification.orderId = @orderid", new { orderId });




    }
}

