using System;
using System.Reflection;
using FluentValidation;
using MediatR;
using POC.Artifacts.Application;
using POC.Artifacts.Domain;
using POC.Artifacts.Domain.Interfaces;

namespace POC.API.Configurations
{
	public static class MediatRConfiguration
	{
        public static WebApplicationBuilder AddMediatRConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            var assembly = AppDomain.CurrentDomain.Load("POC.Domain");

            AssemblyScanner
            .FindValidatorsInAssembly(assembly)
                .ForEach(result => builder.Services.AddScoped(result.InterfaceType, result.ValidatorType));

            builder.Services.AddScoped<IDomainNotificationContext, DomainNotificationContext>();
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));

            return builder;
        }
    }
}

