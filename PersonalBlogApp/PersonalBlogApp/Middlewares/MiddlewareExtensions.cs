namespace PersonalBlogApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder builder) {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
}
