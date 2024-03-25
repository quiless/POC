using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Enumerations;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Order;

namespace POC.Infrastructure.SQLRepositories.Order
{

    public class OrderRepository : RepositoryBase<POC.Domain.Models.Entities.Order>, IOrderRepository
    {
        private readonly SQLDbContextBase _dBContext;

        public OrderRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<IEnumerable<POC.Domain.Models.Entities.Order>> GetAvailableOrders() =>
            await _dbContext.
                    Connection.
                    QueryAsync<POC.Domain.Models.Entities.Order>("SELECT id, price, orderdate, createdbypersonid from public.order where orderstatusid = @statusid", new { statusid = OrderStatusEnum.Available.Id });


        public async Task UpdateOrderAccepted(int orderId, int motorcycleRentalId) =>
             await _dbContext.
                    Connection.
                    QueryAsync<POC.Domain.Models.Entities.Order>(@"UPDATE public.order

                                                                    SET orderstatusid = @statusid,
                                                                    accepteddate = @date,

                                                                    motorcyclerentalid = @motorcycleRentalId

                                                                    WHERE id = @orderId",
                                                                new {
                                                                    statusid = OrderStatusEnum.Accepted.Id,
                                                                    date = DateTime.Now,
                                                                    motorcycleRentalId,
                                                                    orderId }, transaction: _dbContext.Transaction);


        public async Task UpdateOrderFinished(int orderId) =>
            await _dbContext.
                    Connection.
                    QueryAsync<POC.Domain.Models.Entities.Order>(@"UPDATE public.order

                                                                    SET orderstatusid = @statusid,
                                                                    delivereddate = @date

                                                                    WHERE id = @orderId",
                                                                new
                                                                {
                                                                    statusid = OrderStatusEnum.Delivered.Id,
                                                                    date = DateTime.Now,
                                                                    orderId
                                                                }, transaction: _dbContext.Transaction);



        public async Task<IEnumerable<OrdersRegistered>> GetOrders(int? orderStatusId, string CNPJ, DateTime? startDate, DateTime? endDate)
        {
            var _sql = @"SELECT 

                            ordertable.id,
                            ordertable.createdate,
                            price, 
                            orderstatusid,
                            orderdate,
                            accepteddate,
                            delivereddate, 
                            deliveryman.name as DeliverymanName,
                            deliveryman.cnpj as DeliverymanCNPJ, 
                            motorcycle.year as MotorcycleYear,
                            motorcycle.plate as MotorcyclePlate,
                            motorcyclebrand.name as MotorCycleBrandName,
                            motorcyclemodel.name as MotorCycleModelName

                            FROM 

                            public.order ordertable

                            left join motorcyclerental motorcyclerental
                            on motorcyclerental.Id = ordertable.motorcyclerentalid

                            left join deliveryman deliveryman
                            on deliveryman.Id = motorcyclerental.deliverymanId

                            left join motorcycle motorcycle
                            on motorcycle.Id = motorcyclerental.motorcycleId

                            left join motorcyclebrand motorcyclebrand
                            on motorcyclebrand.Id = motorcycle.BrandId

                            left join motorcyclemodel motorcycleModel
                            on motorcycleModel.Id = motorcycle.ModelId

                            WHERE 1 = 1";


            if (orderStatusId > 0)
                _sql += " AND (ordertable.orderstatusid = @orderStatusId)";

            if (!String.IsNullOrEmpty(CNPJ))
                _sql += " AND (deliveryman.CNPJ like @CNPJ)";

            if (startDate != null)
                _sql += " AND (ordertable.orderdate >= @startDate)";

            if (endDate != null)
                _sql += " AND (ordertable.orderdate <= @endDate)";


            return await _dbContext.
                  Connection.
                  QueryAsync<OrdersRegistered>(_sql, new { orderStatusId, CNPJ = "%" + CNPJ + "%", startDate, endDate });
        }
          





    }
}

