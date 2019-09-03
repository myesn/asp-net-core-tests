using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IStartupFilter, A>();
            //services.AddSingleton<IStartupFilter, B>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Console.WriteLine("configure");

            var pathBase = _configuration.GetValue<string>("PathBase");
            app.UsePathBase(pathBase);

            app.Use(next =>
            {
                Console.WriteLine("A");
                return async (context) =>
                {
                    // 1. 对Request做一些处理
                    // TODO

                    // 2. 调用下一个中间件
                    Console.WriteLine("A-BeginNext");

                    await next(context);

                    //context.Response.ContentType = "text/html";

                    //await context.Response.WriteAsync($"Path: {context.Request.Path}<br/>");
                    //await context.Response.WriteAsync($"PathBase: {context.Request.PathBase}<br/>");
                    
                    Console.WriteLine("A-EndNext");

                    // 3. 生成 Response
                    //TODO
                };
            });

            app.UseWhen(x => x.Request.HasFormContentType, appBuilder => appBuilder.UseCors());

            app.MapWhen(
                context => context.Request.Path.StartsWithSegments("/assets"),
                appBuilder => appBuilder.UseStaticFiles());


            app.Map("/account", builder =>
            {
                builder.Run(async context =>
                {
                    Console.WriteLine($"PathBase: {context.Request.PathBase}, Path: {context.Request.Path}");
                    await context.Response.WriteAsync("This is from account");
                });
            });

            app.Run(async context =>
            {
                Console.WriteLine($"PathBase: {context.Request.PathBase}, Path: {context.Request.Path}");
                await context.Response.WriteAsync("This is default");
            });

            app.Run(async context =>
            {
                Console.WriteLine("B");

                Console.WriteLine("B-BeginNext");
                await context.Response.WriteAsync("Hello ASP.NET Core");
                Console.WriteLine("B-EndNext");

            });

            // 不会被执行
            //app.Use(next =>
            //{
            //    Console.WriteLine("B");
            //    return async (context) =>
            //    {
            //        // 1. 对Request做一些处理
            //        // TODO

            //        // 2. 调用下一个中间件
            //        Console.WriteLine("B-BeginNext");
            //        await context.Response.WriteAsync("Hello ASP.NET Core");
            //        Console.WriteLine("B-EndNext");

            //        // 3. 生成 Response
            //        //TODO
            //    };
            //});


        }
    }
}
