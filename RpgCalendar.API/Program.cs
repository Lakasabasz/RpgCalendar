using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RpgCalendar.API;
using RpgCalendar.Database;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var connectionString =
    $"Server={EnvironmentData.RelationalDbHost};Port=3306;Database=rpgcalendar;User ID=root;Password={EnvironmentData.RelationalDbPasswd};";

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<RelationalDb>()
        .WithParameter(
            new TypedParameter(typeof(DbContextOptions),
            new DbContextOptionsBuilder()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options));
});

builder.Logging.ClearProviders();

builder.Services.AddSerilog(configuration => configuration
    .WriteTo.Console()
    .WriteTo.Graylog(new GraylogSinkOptions()
    {
        HostnameOrAddress = EnvironmentData.GraylogUrl,
        MinimumLogEventLevel = LogEventLevel.Verbose,
        TransportType = TransportType.Tcp
    })
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();