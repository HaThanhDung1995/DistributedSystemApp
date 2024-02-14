using Serilog;
using SeriLogThemesLibrary;
using DistributedSystem.Application.DependencyInjection.Extensions;
using DistributedSystem.Infrastructure.DependencyInjection.Extensions;
using DistributedSystem.API.DependencyInjection.Extensions;
using DistributedSystem.API.Middleware;
using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using DistributedSystem.Persistence.DependencyInjection.Extensions;
using DistributedSystem.Persistence.DependencyInjection.Options;
var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build();
// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(configuration)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", theme: SeriLogCustomThemes.Theme1())
    .CreateLogger();
var data = Log.Logger;
builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();
builder.Services.AddServicesInfrastructure();
builder.Services.AddRedisInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthenticationAPI(builder.Configuration);
builder.Services.AddConfigureMediart();
builder.Services.AddConfigureAutoMapper();
//Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddInterceptorPersistence();
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlServerPersistence();
builder.Services.AddRepositoryPersistence();
//Carter
builder.Services.AddCarter();

//Swagger
builder.Services
    .AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwagger()
    ;
builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
}



try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
public partial class Program { }
