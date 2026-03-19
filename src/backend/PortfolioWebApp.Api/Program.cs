using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Api.Logging;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.Services.Categories;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Infrastructure.Data;
using PortfolioWebApp.Infrastructure.Repositories;
using PortfolioWebApp.Api.Middleware;
using PortfolioWebApp.Api.Services;
using PortfolioWebApp.Application.Interfaces;
using Serilog;


// Creates logger that will be replaced once app starts
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new PortfolioConsoleFormatter())
            .WriteTo.File(new PortfolioJsonFormatter(),
                "logs/log-.log",
                rollingInterval: RollingInterval.Day
            );
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IRequestContext, HttpRequestContext>();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();

    var app = builder.Build();
    app.Logger.LogInformation("Environment is {EnvironmentName}", app.Environment.EnvironmentName);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

    app.UseMiddleware<RequestContextLoggingMiddleware>();

    app.UseAuthorization();
    app.MapControllers();
    

    app.Logger.LogInformation("Starting PortfolioWebApp API");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "PortfolioWebApp API failed to start");
}
finally
{
    Log.CloseAndFlush();
}