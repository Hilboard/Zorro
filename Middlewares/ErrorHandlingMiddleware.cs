using System.Text.Json;

namespace Zorro.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (QueryException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.statusCode;

            var result = JsonSerializer.Serialize(new
            {
                title = exception.Message,
                status = exception.statusCode,
                errors = exception.Data.Count > 0 ? exception.Data : null
                //details = exception.Message
            });

            await context.Response.WriteAsync(result);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            //exception.Dump();
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine(exception);

            var result = JsonSerializer.Serialize(new
            {
                title = "An unexpected error occurred.",
                status = StatusCodes.Status500InternalServerError,
                //details = exception.Message
            });

            await context.Response.WriteAsync(result);
        }
    }
}

public class QueryException : Exception
{
    public int statusCode { get; init; }

    public const string STANDART_ERROR_TITLE = "An error occurred while processing your request.";

    public QueryException(
        string title = STANDART_ERROR_TITLE,
        int statusCode = StatusCodes.Status400BadRequest,
        params (string field, string[] messages)[] fields) : base(title)
    {
        this.statusCode = statusCode;
        foreach (var error in fields)
        {
            Data.Add(error.field, error.messages);
        }
    }
}