using System.Net;
using System.Security.Claims;
using POC.API.Configurations;
using POC.Infrastructure;
using POC.Domain;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using IdentityServer4.Services;
using POC.Domain.Models.ModelSettings;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");


builder.Configuration.AddEnvironmentVariables();

builder.Configuration
        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.{env}.json",
            optional: true,
            reloadOnChange: true)
        .AddEnvironmentVariables();



builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization();
builder.Services.AddVersionedApiExplorer();
builder.Services.AddAzureBlobStorage(builder.Configuration);
builder.Services.SetIdentityServerConfiguration(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.Configure<PubnubSetting>(builder.Configuration.GetSection("PubnubSetting"));
builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("DOTNET RUN", LogLevel.Information);

builder.Services.SetDomainBootstrapper();
builder.Services.SetInfrastructureBootstrapper();

builder.AddMediatRConfiguration();
builder.AddThrottleConfiguration();
builder.AddSwaggerConfiguration();
builder.AddApiVersionConfiguration();



var app = builder.Build();

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
IdentityModelEventSource.ShowPII = true;

app.UseHttpsRedirection();
app.UseLocalizationConfiguration();
app.UseThrottler();
app.UseSecurityHeaders();
app.MapControllers().RequireAuthorization();
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1.0/swagger.json", "POC Mottu");
    s.DefaultModelsExpandDepth(-1);
});

app.UseDeveloperExceptionPage();
app.UseIdentityServer();
app.UseAuthentication();
app.Run();




public partial class Program
{
    protected Program() { }
}