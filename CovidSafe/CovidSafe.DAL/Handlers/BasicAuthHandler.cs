using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace CovidSafe.DAL.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly Services.IUserService _userService;

        private const string BasicPrefix = "Basic ";

        public BasicAuthHandler(
            Microsoft.Extensions.Options.IOptionsMonitor<AuthenticationSchemeOptions> options,
            Microsoft.Extensions.Logging.ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder,
            ISystemClock clock,
            Services.IUserService userService)
        : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!DecodeAuthHeader(out var username, out var password, out var result))
            {
                return result;
            }

            var user = await _userService.Authenticate(username, password);
            if (user == null)
            {
                return AuthenticateResult.Fail(Entities.Validation.Resources.ValidationMessages.AccessDenied);
            }
            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private bool DecodeAuthHeader(out string username, out string password, out AuthenticateResult result)
        {
            username = string.Empty;
            password = string.Empty;
            result = null;

            if (!this.Request.Headers.ContainsKey("Authorization")) {
                result = AuthenticateResult.Fail(Entities.Validation.Resources.ValidationMessages.MissingAuthenticationHeader);
                return false;
            }
            string header = this.Request.Headers["Authorization"];
            if (header == null || !header.StartsWith(BasicPrefix))
            {
                result = AuthenticateResult.Fail(Entities.Validation.Resources.ValidationMessages.AuthMethodNotSupported);
                return false;
            }

            byte[] decoded = Convert.FromBase64String(header.Substring(BasicPrefix.Length));
            string decodedStr = Encoding.UTF8.GetString(decoded);

            var fields = decodedStr.Split(':');
            if (fields.Length != 2)
            {
                result = AuthenticateResult.Fail(Entities.Validation.Resources.ValidationMessages.AuthMethodNotSupported);
                return false;
            }

            username = fields[0];
            password = fields[1];

            return true;
        }
    }
}
