using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL;
using POC.Artifacts.SQL.Transactions;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Domain.Queries;
using POC.Domain.Queries.Deliverymans;
using POC.Domain.Queries.Motorcycle;
using POC.Domain.Queries.Users;
using POC.Domain.QueriesHandlers;
using POC.Domain.QueriesHandlers.Deliverymans;
using POC.Domain.QueriesHandlers.Motorcycle;
using POC.Domain.QueriesHandlers.Users;
using POC.Domain.Models.Context;
using POC.Infrastructure.SQLRepositories.Helpers;
using POC.Domain.CommandsHandlers;
using POC.Domain.Commands;
using POC.Domain.Models.Entities;
using POC.Domain.Commands.Motorcycle;
using POC.Domain.Commands.Deliveryman;
using POC.Domain.Models.Aggregators;
using POC.Domain.Commands.Rental;
using POC.Domain.Queries.Rental;
using POC.Domain.QueriesHandlers.Rental;
using POC.Domain.Commands.Order;
using POC.Domain.Queries.Order;
using POC.Domain.QueriesHandlers.Order;

namespace POC.Domain
{
    public static class Bootstrapper
    {
        public static void SetDomainBootstrapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            SqlRegisterMappings.Register();

            services.AddScoped<ApplicationContextBase, ApplicationContext>();
            services.AddScoped<IRequestHandler<GetDriverLicenseTypeQuery, GenericCommandResult>, DeliverymanQueryHandler>();

            #region Users

            services.AddScoped<IRequestHandler<ValidateUserAuthenticationQuery, bool>, UserQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserByUsernameQuery, UserLoggedInfo>, UserQueryHandler>();

            #endregion


            #region MotorcycleModel

            services.AddScoped<IRequestHandler<GetMotorcycleModelByIdQuery, MotorcycleModel>, MotorcycleModelQueryHandler>();
            services.AddScoped<IRequestHandler<GetMotorcycleModelByNameQuery, MotorcycleModel>, MotorcycleModelQueryHandler>();
            services.AddScoped<IRequestHandler<GetMotorcycleModelsQuery, GenericCommandResult>, MotorcycleModelQueryHandler>();


            #endregion

            #region MotorcycleBrand

            services.AddScoped<IRequestHandler<GetMotorcycleBrandByIdQuery, MotorcycleBrand>, MotorcycleBrandQueryHandler>();
            services.AddScoped<IRequestHandler<GetMotorcycleBrandByNameQuery, MotorcycleBrand>, MotorcycleBrandQueryHandler>();
            services.AddScoped<IRequestHandler<GetMotorcycleBrandsQuery, GenericCommandResult>, MotorcycleBrandQueryHandler>();

            #endregion


            #region Motorcycle

            services.AddScoped<IRequestHandler<CheckMotorcycleByPlateQuery, Motorcycle>, MotorcycleQueryHandler>();
            services.AddScoped<IRequestHandler<RegisterMotorcycleCommand, GenericCommandResult>, MotorcycleCommandHandler>();
            services.AddScoped<IRequestHandler<SearchMotorcyclesQuery, GenericCommandResult>, MotorcycleQueryHandler>();
            services.AddScoped<IRequestHandler<GetMotorcyclesQuery, GenericCommandResult>, MotorcycleQueryHandler>();
            services.AddScoped<IRequestHandler<RefreshMotorcycleCommand, GenericCommandResult>, MotorcycleCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveMotorcycleCommand, GenericCommandResult>, MotorcycleCommandHandler>();

            #endregion


            #region Deliveryman

            services.AddScoped<IRequestHandler<RegisterDeliverymanCommand, GenericCommandResult>, DeliverymanCommandHandler>();
            services.AddScoped<INotificationHandler<CheckDeliverymanByDriverLicenseNumberQuery>,DeliverymanQueryHandler>();
            services.AddScoped<INotificationHandler<CheckDeliverymanByCNPJQuery>, DeliverymanQueryHandler>();
            services.AddScoped<IRequestHandler<RefreshDeliverymanDriverLicenseFileCommand, GenericCommandResult>, DeliverymanCommandHandler>();
            services.AddScoped<IRequestHandler<GeDeliverymanDriverLicenseFileQuery, GenericCommandResult>, DeliverymanQueryHandler>();
            services.AddScoped<IRequestHandler<GetDeliverymanInfoQuery, GenericCommandResult>, DeliverymanQueryHandler>();
            services.AddScoped<INotificationHandler<SetDeliverymanOpenRentCommand>, DeliverymanCommandHandler>();
            services.AddScoped<INotificationHandler<SetDeliverymanCloseRentCommand>, DeliverymanCommandHandler>();
            services.AddScoped<IRequestHandler<GetDeliverymansAvailableQuery, IEnumerable<DeliverymanAvailable>>, DeliverymanQueryHandler>();


            #endregion

            #region RentMotorcycle
           

            services.AddScoped<IRequestHandler<FinalizedRentMotorcycleCommand, GenericCommandResult>, RentMotorcycleCommandHandler>();
            services.AddScoped<IRequestHandler<RentMotorcycleCommand, GenericCommandResult>, RentMotorcycleCommandHandler>();
            services.AddScoped<IRequestHandler<CheckRentalPriceQuery, GenericCommandResult>, RentMotorcycleQueryHandler>();
            services.AddScoped<IRequestHandler<GetRentalMotorcyclePlansQuery, GenericCommandResult>, RentMotorcycleQueryHandler>();

            #endregion

            #region Order

            services.AddScoped<IRequestHandler<RegisterOrderCommand, GenericCommandResult>,OrderCommandHandler>();
            services.AddScoped<INotificationHandler<NotifyDelivermansCommand>, OrderCommandHandler>();
            services.AddScoped<INotificationHandler<NotifyDeliveriesPendingOrdersCommand>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<OrderAcceptedCommand,GenericCommandResult>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<OrderFinishedCommand, GenericCommandResult>, OrderCommandHandler>();

            services.AddScoped<IRequestHandler<SearchOrdersQuery, GenericCommandResult>, OrderQueryHandler>();
            services.AddScoped<IRequestHandler<GetOrderNotificationsQuery, GenericCommandResult>, OrderQueryHandler>();

            #endregion

        }
    }
}


