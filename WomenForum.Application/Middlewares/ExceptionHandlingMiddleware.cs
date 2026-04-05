using WomenForum.Exceptions;

namespace WomenForum.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, ex.Message);
            
            await HandleExceptionAsync(context, ex.Message,ex.StatusCode);
        }
        catch (NoPermissionException ex)
        {
            logger.LogError(ex, ex.Message);
            
            await HandleExceptionAsync(context, ex.Message,ex.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            
            await HandleExceptionAsync(context, ex.Message, 500);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, string errorMessage, int statusCode)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        response.StatusCode = statusCode;

        return response.WriteAsync(errorMessage);
    }
}