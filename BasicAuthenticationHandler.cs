using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using BCrypt.Net;

namespace art_gallery_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserDataAccess _userDataAccess;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserDataAccess userDataAccess) : base(options, logger, encoder, clock)
        {
            _userDataAccess = userDataAccess;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // If the endpoint has [AllowAnonymous], skip authentication.
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller"", charset=""UTF-8""");
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = credentials.Split(':', 2);

            if (parts.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var email = parts[0];
            var password = parts[1];
            var user = _userDataAccess.GetUsers().FirstOrDefault(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}

