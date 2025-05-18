namespace PersonalBlogApp.Middlewares
{
    // **Tạo Extension Method để Đăng ký Middleware**

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
