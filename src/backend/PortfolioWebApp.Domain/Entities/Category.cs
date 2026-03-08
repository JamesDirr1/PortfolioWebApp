namespace PortfolioWebApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public int DisplayOrder { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;
}