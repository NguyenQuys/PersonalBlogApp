using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalBlogApp.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = user.Identity.Name;
                var avatar = user.FindFirst("AvatarUrl")?.Value;

                context.Items["CurrentUserId"] = userId;
                context.Items["CurrentUserName"] = userName;
                context.Items["CurrentUserAvatar"] = avatar;
            }

            await _next(context);
        }
    }
}
