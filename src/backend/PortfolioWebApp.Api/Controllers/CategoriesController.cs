using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Utilities;
using PortfolioWebApp.Application.Interfaces.Categories;


namespace PortfolioWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService,
        ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        _logger.LogInformation("Returned {Count} categories", categories.Count());
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Categories JSON: {CategoriesJson}", JsonLogHelper.ToJson(categories));
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting category with id {id}", id);
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            _logger.LogWarning("Category with id {id} not found", id);
            return NotFound(new { message = "Category not found" });
        }

        _logger.LogInformation("Returned category with id {id}", id);
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug("Category JSON: {CategoryJson}", JsonLogHelper.ToJson(category));

        return Ok(category);
    }
}