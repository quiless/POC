using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Repositories.Interfaces;
using POC.Artifacts.SQL.Transactions;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Infrastructure.SQLRepositories;
using POC.Infrastructure.SQLRepositories.Deliveryman;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Order;
using POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle;
using POC.Infrastructure.SQLRepositories.Motorcycle;
using POC.Infrastructure.SQLRepositories.Order;
using POC.Infrastructure.SQLRepositories.RentMotorcycle;

namespace POC.Infrastructure
{
    public static class Bootstrapper
    {
        public static void SetInfrastructureBootstrapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));


            services.AddScoped<SQLDbContextBase, SqlContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderNotificationRepository, OrderNotificationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRentMotorcycleRepository, RentMotorcycleRepository>();
            services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
            services.AddScoped<IDriverLicenseRepository, DriverLicenseRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IMotorcycleBrandRepository, MotorcycleBrandRepository>();
            services.AddScoped<IMotorcycleModelRepository, MotorcycleModelRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}