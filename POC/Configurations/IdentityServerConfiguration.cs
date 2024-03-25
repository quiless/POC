using System;
using System.Buffers;
using System.Runtime.Versioning;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using POC.API.Security;
using POC.API.Services;
using POC.Domain.Models.ModelSettings;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace POC.API.Configurations
{
    public static class IdentityServerConfig
    {
        public static IServiceCollection SetIdentityServerConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            var appSettingsSection = configuration.GetSection("AuthenticationSetting");
            services.Configure<AuthenticationSetting>(appSettingsSection);
            var authSettings = appSettingsSection.Get<AuthenticationSetting>();

            var builder = services.AddIdentityServer()
                                  .AddInMemoryClients(new List<Client>()
                                  {
                                      new Client()
                                      {
                                           ClientId = authSettings.ClientId,
                                           ClientName = authSettings.ClientName,
                                           AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword},
                                           ClientSecrets = { new IdentityServer4.Models.Secret(authSettings.ClientSecret.Sha256()) },
                                           AccessTokenLifetime = 3600,
                                           RequireConsent = false,
                                           AllowedScopes = authSettings.Scope.Split(',').ToList(),
                                           AllowOfflineAccess = true,
                                           SlidingRefreshTokenLifetime = 3600,
                                           RefreshTokenExpiration = TokenExpiration.Sliding,
                                           RefreshTokenUsage = TokenUsage.ReUse
                                      }
                                  })
                                  .AddInMemoryApiScopes(authSettings.Scope.Split(',').ToList().Select(x => new ApiScope() { Name = x }))
                                  .AddResourceStore<InMemoryResourcesStore>()
                                  .AddProfileService<ProfileService>()
                                  .AddResourceOwnerValidator<ResourceOwnerPasswordValidatorCustom>();

            builder.AddDeveloperSigningCredential();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication("Mottu")
                 .AddPolicyScheme("Mottu", "Authorization Default", options =>
                 {
                     options.ForwardDefaultSelector = context =>
                     {
                         return "Bearer";
                     };
                 })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authSettings.Authority;
                    options.MetadataAddress = authSettings.Authority + "/.well-known/openid-configuration";
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("Mottu", policy =>
                {
                    policy.AddAuthenticationSchemes(new string[] { "Bearer" });
                    policy.RequireAuthenticatedUser();

                });

                options.DefaultPolicy = options.GetPolicy("Mottu");
            });



            return services;
        }
    }

}

