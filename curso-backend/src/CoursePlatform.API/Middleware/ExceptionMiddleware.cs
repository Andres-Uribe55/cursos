using System.Net;
using System.Text.Json;
using CoursePlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        
        var statusCode = HttpStatusCode.InternalServerError;
        var title = "Server Error";
        var detail = exception.Message;

        switch (exception)
        {
            case CourseNotFoundException or LessonNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                title = "Not Found";
                break;
            case CannotPublishCourseException or DuplicateLessonOrderException:
                statusCode = HttpStatusCode.BadRequest;
                title = "Business Rule Violation";
                break;
            case DomainException:
                statusCode = HttpStatusCode.BadRequest;
                title = "Bad Request";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, options);

        await context.Response.WriteAsync(result);
    }
}