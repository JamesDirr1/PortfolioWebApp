using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Responses;
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

    builder.Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value is not null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors
                            .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                                ? "Validation error."
                                : e.ErrorMessage)
                            .ToArray());
                Log.Warning("Validation failed. Errors: {@ValidationErrors}", errors);
                var response = ApiValidationResponse.Failure(
                    "Validation failed.",
                    errors);

                return new BadRequestObjectResult(response);
            };
        });

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

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

            var logger = context.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("GlobalExceptionHandler");

            if (exceptionFeature?.Error is not null)
            {
                logger.LogError(exceptionFeature.Error, "Unhandled exception occurred.");
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.FailureResponse(
                "An unexpected error occurred.",
                ["Please try again later."]);

            await context.Response.WriteAsJsonAsync(response);
        });
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

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