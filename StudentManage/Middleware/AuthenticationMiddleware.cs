using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StudentManage.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Session.Keys.Contains("UserEmail") && context.Request.Path.StartsWithSegments("/student"))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}
