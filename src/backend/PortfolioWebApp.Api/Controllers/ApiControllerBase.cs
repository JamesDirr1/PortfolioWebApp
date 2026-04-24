using Microsoft.AspNetCore.Mvc;
using PortfolioWebApp.Api.Responses;


namespace PortfolioWebApp.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(T data, string message = "Request successful.")
    {
        var response = ApiResponse<T>.SuccessResponse(data, message);
        return Ok(response);
    }

    protected ActionResult<ApiResponse<T>> Failure<T>(string message = "Request failed.", params string[] errors)
    {
        var response = ApiResponse<T>.FailureResponse(message, errors);
        return BadRequest(response);
    }
}