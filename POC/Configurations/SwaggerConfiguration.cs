using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace POC.API.Configurations
{
	public static class SwaggerConfiguration
	{
        public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
          
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds((x) =>
                {
                    try
                    {
                        var attribute = x.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault();
                        return attribute == null ? x.Name : attribute.DisplayName;
                    }
                    catch
                    {
                        return x.Name;
                    }
                });

                swagger.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                    IEnumerable<ApiVersion> versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(a => a.Versions);
                    #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.

                    return versions.Any(v => $"v{v}" == docName);
                });

                swagger.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Mottu API",
                    Description = "Desafio backend Mottu",
                    Contact = new OpenApiContact
                    {
                        Name = "Jéfferson Quiles",
                        Email = "jehwl3@hotmail.com"
                    }
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Autenticação baseada em Json Web Token (JWT).",
                    Type = SecuritySchemeType.Http
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                //swagger.OperationFilter<FileFormOperation>();

                SetXmlDocumentation(swagger);
            });

            return builder;
        }

        private static void SetXmlDocumentation(SwaggerGenOptions options)
        {
            foreach (var file in GetXmlDocumentFiles())
            {
                options.IncludeXmlComments(file, true);
            }
        }
        private static IEnumerable<string> GetXmlDocumentFiles() =>
            Directory.GetFiles(string.Format("{0}/", AppContext.BaseDirectory), "*.xml");
    }
}

