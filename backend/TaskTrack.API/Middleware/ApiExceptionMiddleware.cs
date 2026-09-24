using System.Text.Json;
using TaskTrack.Service.Exceptions;

namespace TaskTrack.API.Middleware;

public class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (ServiceException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            object body = ex.Errors.Count > 0
                ? new { message = ex.Message, errors = ex.Errors }
                : new { message = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled API exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected server error occurred." });
        }
    }
}
