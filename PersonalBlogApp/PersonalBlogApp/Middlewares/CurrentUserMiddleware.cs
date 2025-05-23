using Microsoft.AspNetCore.Identity;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(context.User);
                var isAdmin = context.User.IsInRole("Admin");

                context.Items["UserId"] = userId;
                context.Items["IsAdmin"] = isAdmin;
            }

            await _next(context);
        }
    }
}
