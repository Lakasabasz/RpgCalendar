using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RpgCalendar.API;
using RpgCalendar.API.Middlewares;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs;
using RpgCalendar.Database;
using RpgCalendar.Tools;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var connectionString =
    $"Server={EnvironmentData.RelationalDbHost};Port=3306;Database=rpgcalendar;Uid=root;Pwd={EnvironmentData.RelationalDbPasswd};";

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<RelationalDb>()
        .WithParameter(
            new TypedParameter(typeof(DbContextOptions),
            new DbContextOptionsBuilder()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options));
    container.RegisterType<InjectUserMiddleware>();
    container.RegisterAssemblyTypes(typeof(IJob).Assembly)
        .Where(x => x.GetInterface(nameof(IJob)) is not null)
        .AsImplementedInterfaces()
        .AsSelf();
    container.RegisterType<ImageService>();
    container.RegisterType<GroupService>();
});

builder.Logging.ClearProviders();

builder.Services.AddSerilog(configuration => configuration
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.Graylog(new GraylogSinkOptions()
    {
        HostnameOrAddress = EnvironmentData.GraylogUrl,
        MinimumLogEventLevel = LogEventLevel.Verbose,
        TransportType = TransportType.Tcp
    })
);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(x =>
{
    x.LoggingFields = HttpLoggingFields.All;
    x.RequestBodyLogLimit = 4 * 1024;
    x.ResponseBodyLogLimit = 4 * 1024;
    x.CombineLogs = true;
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.MapInboundClaims = false;
    x.Authority = $"{EnvironmentData.KeycloakInternalUrl}/realms/{EnvironmentData.KeycloakRealm}";
    x.Audience = EnvironmentData.KeycloakAudience;
    x.RequireHttpsMetadata = false;
    x.MetadataAddress = $"{EnvironmentData.KeycloakInternalUrl}/realms/{EnvironmentData.KeycloakRealm}/.well-known/openid-configuration";

    FeatureFlag.RequireFeatureFlag(FeatureFlag.FeatureFlagEnum.KEYCLOAK_CERT, () =>
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        x.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            x.MetadataAddress, new OpenIdConnectConfigurationRetriever(), new HttpClient(handler));
    });
});

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<ExceptionFallback>();

var app = builder.Build();

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<RelationalDb>().Database.Migrate();
app.Logger.Log(LogLevel.Information, "Database migrations done");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseUserInjection();

app.MapControllers();

app.Run();