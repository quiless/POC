﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace POC.API.Configurations
{
    ///<Summary>
    /// Versionamento da API
    ///</Summary>
    public static class ApiVersionConfiguration
    {
        ///<Summary>
        /// Adiciona versionamento no formato v0.0
        ///</Summary>
        public static WebApplicationBuilder AddApiVersionConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            object value = builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return builder;
        }
    }
}

