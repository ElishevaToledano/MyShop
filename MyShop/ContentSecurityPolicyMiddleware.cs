using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyShop.Middleware
{
    public class ContentSecurityPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public ContentSecurityPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add(
                "Content-Security-Policy",
                "script-src 'self' 'unsafe-inline' www.google.com; " +
                "default-src 'self'; " +
                "connect-src 'self' wss://localhost:44303 wss://localhost:44358 wss://localhost:44391; " +
                "navigate-to 'none';" +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self'");

            await _next(context);
        }
    }
}