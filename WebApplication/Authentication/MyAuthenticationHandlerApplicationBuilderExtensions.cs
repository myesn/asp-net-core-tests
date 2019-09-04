using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Authentication
{
    public static class MyAuthenticationHandlerApplicationBuilderExtensions
    {
        internal const string _scheme = "myScheme";
        public static IApplicationBuilder UseAuthenticationHanderMiddleware(this IApplicationBuilder app)
        {
            app.Map("/login", builder =>
            {
                builder.Use(next =>
                {
                    return async context =>
                    {
                        var claimIdentity = new ClaimsIdentity();
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
                        await context.SignInAsync(_scheme, new ClaimsPrincipal(claimIdentity));
                    };
                });
            });

            app.Map("/logout", builder =>
            {
                builder.Use(next =>
                {
                    return async context =>
                    {
                        await context.SignOutAsync(_scheme);
                    };
                });
            });

            app.Use(next =>
            {
                return async context =>
                {
                    var result = await context.AuthenticateAsync(_scheme);
                    if (result?.Principal != null)
                    {
                        context.User = result.Principal;
                    }
                    await next(context);
                };
            });

            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user?.Identity?.IsAuthenticated ?? false)
                {
                    if (user?.Identity?.Name != "jim")
                    {
                        await context.ForbidAsync(_scheme);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await context.ChallengeAsync(_scheme);
                }
            });

            app.Map("/resource", builder => 
            {
                builder.Run(async context => 
                {
                    await context.Response.WriteAsync("Hello, ASP.NET Core!");
                });
            });

            return app;
        }
    }
}
