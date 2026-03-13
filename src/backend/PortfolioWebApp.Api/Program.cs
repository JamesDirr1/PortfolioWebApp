using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(new PortfolioJsonFormatter())
    .CreateLogger();


builder.Services.AddSerilog();

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

app.UseMiddleware<RequestGuidMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();