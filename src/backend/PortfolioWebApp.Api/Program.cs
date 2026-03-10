using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.Services.Categories;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Infrastructure.Data;
using PortfolioWebApp.Infrastructure.Repositories;
using PortfolioWebApp.Api.Middleware;
using PortfolioWebApp.Api.Services;
using PortfolioWebApp.Application.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
    options.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
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

app.UseMiddleware<RequestGuidMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();