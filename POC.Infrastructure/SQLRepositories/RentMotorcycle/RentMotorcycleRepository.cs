using System;
using Dapper;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories;
using POC.Domain.Models.Entities;
using POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle;

namespace POC.Infrastructure.SQLRepositories.RentMotorcycle
{
	public class RentMotorcycleRepository: RepositoryBase<MotorcycleRental>, IRentMotorcycleRepository
	{
        private readonly SQLDbContextBase _dBContext;

        public RentMotorcycleRepository(SQLDbContextBase dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        /* Criei dois métodos para buscar plano de locação. Um que buscará apenas o plano caso esteja ativo e outro que buscará o plano independente de estar ativo.
         *
         * São dois cenários:
         * 
         * Cenário 1 -> Um entregar deseja realizar uma nova locação, logo, apenas os planos que estão ativos (isdeleted = false) serão exibidos.
         * 
         * Pensando no cenário onde existem locações em andamento, porém, o time de negócios decide mudar as regras do plano. Exemplo: a diária custava R$ 30,00 e agora passará a custar R$ 32,00.
         * 
         * Aqueles que já alocaram por R$ 30,00 a diária, permanecem com esse valor. O time de negócios atualiza o plano de R$ 30,00 para isdeleted = true, ou seja, não é possível realizar novas locações nesse plano.
         * 
         * No momento que o entregador encerrar a locação, o plano antigo será consultado, independente se estiver ativo ou não, visto que no momento que ele alocou, o plano estava ativo.
         * 
         */

        public async Task<MotorcycleRentalPlan> GetActiveMotorcycleRentalPlanById(int id) =>
           await _dbContext.
               Connection.
               QuerySingleOrDefaultAsync<MotorcycleRentalPlan>(@"SELECT id, totalDays, dailyprice, additionaldailyprice, percentagedailynoteffective FROM MotorcycleRentalPlan WHERE id = @id and isdeleted = false",
                                                               new { id });


        public async Task<MotorcycleRentalPlan> GetMotorcycleRentalPlanById(int id) =>
            await _dbContext.
                Connection.
                QuerySingleOrDefaultAsync<MotorcycleRentalPlan>(@"SELECT id, totalDays, dailyprice, additionaldailyprice, percentagedailynoteffective FROM MotorcycleRentalPlan WHERE id = @id",
                                                                new { id });


        public async Task<IEnumerable<MotorcycleRentalPlan>> GetMotorcycleRentalPlans() =>
            await _dbContext.
                Connection.
                QueryAsync<MotorcycleRentalPlan>(@"SELECT id, description, totalDays, dailyprice, additionaldailyprice, percentagedailynoteffective FROM MotorcycleRentalPlan");


        public async Task RegisterRent(MotorcycleRental rent) =>
            await _dbContext.
                Connection.
                ExecuteAsync(@"INSERT INTO motorcyclerental (startdate, motorcycleid, motorcyclerentalplanid, expectedenddate, deliverymanid, isdeleted, uniqueid, createddate)
                                                     VALUES (@StartDate, @MotorcycleId, @MotorcycleRentalPlanId, @ExpectedEndDate, @DeliverymanId, false, @UniqueId, @CreatedDate)",
                                                     new {
                                                         rent.StartDate,
                                                         rent.MotorcycleId,
                                                         rent.MotorcycleRentalPlanId,
                                                         rent.ExpectedEndDate,
                                                         rent.DeliverymanId,
                                                         rent.UniqueId,
                                                         rent.CreatedDate
                                                     }, transaction: _dbContext.Transaction);

        public async Task<MotorcycleRental> GetActiveRentMotorcycleUserLogged(int deliverymanid) =>
            await _dbContext.
                Connection.
                QuerySingleOrDefaultAsync<MotorcycleRental>(@"SELECT id, uniqueid, expectedenddate, motorcyclerentalplanid, startdate

                                                            FROM motorcyclerental

                                                            WHERE deliverymanid = @deliverymanid
                                                            AND wasfinished = false
                                                            AND isdeleted = false

                                                            LIMIT 1",
                                                            new { deliverymanid });



        public async Task UpdateMotorcycleRentalUnavaliable(int motorcycleRentalId) =>
            await _dbContext.Connection.ExecuteAsync("update motorcycleRental SET  hasopenorder = true, totalorders = totalorders + 1 WHERE id = @id",
                                                new { id = motorcycleRentalId },
                                                transaction: _dbContext.Transaction);


        public async Task UpdateMotorcycleRentalAvaliable(int motorcycleRentalId) =>
            await _dbContext.Connection.ExecuteAsync("update motorcycleRental SET hasopenorder = false WHERE id = @id",
                                                new { id = motorcycleRentalId },
                                                transaction: _dbContext.Transaction);


        public async Task   UpdateMotorcycleRentalFinalized(int motorcycleRentalId, decimal paidvalue, bool fine) =>
            await _dbContext.Connection.ExecuteAsync("update motorcycleRental SET enddate = @enddate, paidvalue = @paidvalue, fine = @fine, wasfinished = true WHERE id = @id",
                                                new { id = motorcycleRentalId, fine, paidvalue, enddate = DateTime.Now },
                                                transaction: _dbContext.Transaction);

        public async Task<MotorcycleRental> GetMotorcycleRentalWithOpenOrderByUserId(int userId) =>
            await _dbContext.Connection.QuerySingleOrDefaultAsync<MotorcycleRental>(@" SELECT motorcycleid, motorcyclerental.id, deliverymanid, motorcyclerental.motorcyclerentalplanid, startdate, expectedenddate

                                                                        FROM motorcyclerental motorcyclerental

                                                                        inner join deliveryman deliveryman 
                                                                        on deliveryman.Id = motorcyclerental.DeliverymanId

                                                                        inner join Userinfo Userinfo
                                                                        on Userinfo.Id = deliveryman.userId

                                                                        where motorcyclerental.wasfinished = false
                                                                        and motorcyclerental.hasopenorder = true
                                                                        and UserInfo.id = @userid", new { id = userId });

        public async Task<MotorcycleRental> GetMotorcycleRentalOpenByUserId(int userId) =>
          await _dbContext.Connection.QuerySingleOrDefaultAsync<MotorcycleRental>(@" SELECT motorcycleid, motorcyclerental.id, deliverymanid, motorcyclerental.motorcyclerentalplanid, startdate, expectedenddate

                                                                        FROM motorcyclerental motorcyclerental

                                                                        inner join deliveryman deliveryman 
                                                                        on deliveryman.Id = motorcyclerental.DeliverymanId

                                                                        inner join Userinfo Userinfo
                                                                        on Userinfo.Id = deliveryman.userId

                                                                        where motorcyclerental.wasfinished = false
                                                                        and UserInfo.id = @userid", new { id = userId });




        public async Task SumTotalAmount(decimal price, int motorcycleRentalId) =>
            await _dbContext.Connection.ExecuteAsync("update motorcycleRental SET totalValueGenerated = totalValueGenerated + @price WHERE id = @id",
                                                new { id = motorcycleRentalId, price },
                                                transaction: _dbContext.Transaction);



    }
}

