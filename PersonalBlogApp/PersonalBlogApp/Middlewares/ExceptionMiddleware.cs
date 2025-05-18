using System.Net;
using System.Text.Json;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Gọi Middleware tiếp theo hoặc Controller
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Unhandled Exception occurred.");

                context.Response.StatusCode = ex switch
                {
                    ArgumentException => 400, 
                    KeyNotFoundException => 404, 
                    UnauthorizedAccessException => 401,
                    _ => 500 
                };

                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = ex.Message,
                    errorType = ex.GetType().Name,
                    timestamp = DateTime.UtcNow,
                    statusCode = context.Response.StatusCode
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
