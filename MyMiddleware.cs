using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication2
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        ILoggerFactory _loggerFactory;
        IOptions<MyMiddlewareOptions> _options;

        public MyMiddleware(RequestDelegate next, ILoggerFactory factory, IOptions<MyMiddlewareOptions> options)
        {
            _next = next;
            _loggerFactory = factory;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _loggerFactory.AddConsole();
            var logger = _loggerFactory.CreateLogger("My Logger");
            logger.LogInformation("My middleware invoked");

            await httpContext.Response.WriteAsync("This is my middleware</br> and option is " + _options.Value.OptionOne);
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }

    public class MyMiddlewareOptions
    {
        public string OptionOne { get; set; }
    }

}
