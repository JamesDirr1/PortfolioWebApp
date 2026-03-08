using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Application.Interfaces.Categories;
using PortfolioWebApp.Application.Services.Categories;
using PortfolioWebApp.Domain.Interfaces;
using PortfolioWebApp.Infrastructure.Data;
using PortfolioWebApp.Infrastructure.Repositories;
using static PortfolioWebApp.Domain.Interfaces.ICategoryRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();