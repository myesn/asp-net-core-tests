using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace HealthCheckTests
{
    /// <summary>
    /// https://www.c-sharpcorner.com/article/implementing-net-core-health-checks/
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("sql");
            //.AddCheck("sql",()=> 
            //{
            //    using var connection = new SqlConnection("");
            //    try
            //    {
            //        connection.Open();
            //    }
            //    catch (SqlException)
            //    {
            //        return HealthCheckResult.Unhealthy();
            //    }

            //    return HealthCheckResult.Healthy();
            //});

            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseHealthChecks("/Health");

            app.UseHealthChecksUI();
        }
    }
}
