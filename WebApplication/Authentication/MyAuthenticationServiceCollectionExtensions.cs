using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyAuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddMyAuthenticationHandler(this IServiceCollection services)
        {
            return services
                .AddAuthenticationCore(options =>
                {
                    options.AddScheme<MyAuthenticationHandler>("mySchema", "my demo schema");
                });
        }
    }
}
