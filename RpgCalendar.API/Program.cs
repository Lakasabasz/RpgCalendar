using Autofac;
using Autofac.Extensions.DependencyInjection;
using RpgCalendar.API;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();