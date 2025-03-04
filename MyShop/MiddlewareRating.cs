using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using service;
using System;
using System.Threading.Tasks;

namespace MyShop
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MiddlewareRating
    {
        private readonly RequestDelegate _next;

        public MiddlewareRating(RequestDelegate next)
        {
            _next = next;
        }

        public  async Task Invoke(HttpContext httpContext, IRatingService ratingService)
        {
            //            •	HOST - כתובת האתר בה אנו גולשים כעת
            //•	METHOD - המתודה אליה נגשנו)
            //•	[PATH] URL ה-אליו בוצעה הפניה
            //•	REFERER - הדף ממנו התבצעה הפניה
            //•	USER_AGENT - מכיל את שם הדפדפן, גירסתו, מערכת ההפעלה ושפתה
            //•	RECORD_DATE - תאריך הרישום לרייטינג
            Rating ratin = new();
            ratin.Method = httpContext.Request.Method;
            ratin.Host = httpContext.Request.Host.ToString();
            ratin.Path = httpContext.Request.Path.Value;
            ratin.UserAgent = httpContext.Request.Headers.UserAgent;
            ratin.Referer = httpContext.Request.Headers.Referer;
            ratin.RecordDate = DateTime.Now;
            await  ratingService.AddRating(ratin);
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareRatingExtensions
    {
        public static IApplicationBuilder UseMiddlewareRating(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareRating>();
        }
        
    }
}
