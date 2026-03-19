namespace PortfolioWebApp.Application.Interfaces;

public interface IRequestContext
{
    string GetRequestId { get; }
}