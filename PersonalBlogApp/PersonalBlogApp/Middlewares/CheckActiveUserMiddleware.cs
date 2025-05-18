//using PersonalBlogApp.Services;

//namespace PersonalBlogApp.Middlewares
//{
//    public class CheckActiveUserMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public CheckActiveUserMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var userService = context.RequestServices.GetRequiredService<IUserService>();
//            await userService.CheckActiveUser();

//            await _next(context);
//        }
//    }
//}
