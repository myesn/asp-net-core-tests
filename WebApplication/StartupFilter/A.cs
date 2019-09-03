using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class A : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Console.WriteLine("A1");

            return app => 
            {
                Console.WriteLine("A2");
                next(app);
            };
        }
    }

    public class B : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Console.WriteLine("B1");

            return app =>
            {
                Console.WriteLine("B2");
                next(app);
            };
        }
    }
}
