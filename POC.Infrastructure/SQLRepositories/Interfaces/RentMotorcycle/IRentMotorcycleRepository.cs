using System;
using MediatR;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Domain.Models.Context;
using POC.Domain.Models.Entities;

namespace POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle
{
	public interface IRentMotorcycleRepository: IRepository<MotorcycleRental>
    {
		Task<MotorcycleRentalPlan> GetMotorcycleRentalPlanById(int id);
        Task<MotorcycleRentalPlan> GetActiveMotorcycleRentalPlanById(int id);
        Task RegisterRent(MotorcycleRental rent);
        Task<MotorcycleRental> GetActiveRentMotorcycleUserLogged(int userId);
        Task<IEnumerable<MotorcycleRentalPlan>> GetMotorcycleRentalPlans();
        Task UpdateMotorcycleRentalUnavaliable (int motorcycleRentalId);
        Task UpdateMotorcycleRentalAvaliable (int motorcycleRentalId);
        Task<MotorcycleRental> GetMotorcycleRentalWithOpenOrderByUserId(int userId);
        Task<MotorcycleRental> GetMotorcycleRentalOpenByUserId(int userId);
        Task SumTotalAmount(decimal price, int motorcycleRentalId);
        Task UpdateMotorcycleRentalFinalized(int motorcycleRentalId, decimal paidvalue, bool fine);
    }
}

