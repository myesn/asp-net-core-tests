using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication
{
    public class MyAuthenticationHandler :
        IAuthenticationHandler,
        IAuthenticationSignInHandler,
        IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        internal const string _cookieName = "mycookie";

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies[_cookieName];
            if (cookie == null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            return Task.FromResult(AuthenticateResult.Success(null));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");

            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = (int)StatusCodes.Status403Forbidden;

            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;

            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var tickt = new AuthenticationTicket(user, properties, Scheme.Name);
            Context.Response.Cookies.Append(_cookieName, null);

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete(_cookieName);

            return Task.CompletedTask;
        }
    }
}
