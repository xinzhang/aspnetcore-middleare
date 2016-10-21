using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddCors();

            services.AddRouting();

            var myoption = Configuration.GetSection("MyMiddlewareOptionsSection");
            services.Configure<MyMiddlewareOptions>(o => o.OptionOne = myoption["OptionOne"]);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.MapWhen()

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello first component.<br/>");
            //    await next.Invoke();
            //    await context.Response.WriteAsync("Hello first by the way.<br/>");
            //});

            app.UseStaticFiles();

            app.UseCors( (builder) =>
            {
                builder.AllowAnyOrigin();
            });

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("greeting/{name}", (appbuilder) =>
            {
                appbuilder.Run(async (context) =>
                {
                    var name = context.GetRouteValue("name");
                   await context.Response.WriteAsync($"Greeting {name} <br/>");
                });
            });

            app.UseRouter(routeBuilder.Build());

            app.UseMyMiddleware();

            //app.Map("/mybooks", (appBuilder) =>
            //{
            //    appBuilder.Use(async (context, next) =>
            //   {
            //       await context.Response.WriteAsync("my books <br/>");
            //       await next.Invoke();
            //   });
            //});

            //app.MapWhen(context => context.Request.Query.ContainsKey("key"), (appBuilder) =>
            //{
            //    appBuilder.Run(async (context) =>
            //    {
            //        await context.Response.WriteAsync("mapped the item <br/>");
            //    });
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World in the last pipeline. <br/>");
            //});

        }
    }
}
