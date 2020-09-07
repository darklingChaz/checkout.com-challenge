



using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentGateway.Models.Auth;

namespace PaymentGateway.Auth {


    public class CustomTokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string CustomTokenAuthenticationSchema = "CustomToken";
    }


    public class CustomAuthenticationHandler : AuthenticationHandler<CustomTokenAuthenticationOptions>
    {
        private readonly ICredentialTokenManager tokenManager;
 
        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomTokenAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            ICredentialTokenManager customAuthenticationManager) 
            : base(options, logger, encoder, clock)
        {
            this.tokenManager = customAuthenticationManager;

        }
 
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("Authorization"))
                return await Task.FromResult(AuthenticateResult.Fail("Unauthorized | Auth token not supplied"));  // single await to remove warn
 
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }
 
            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
 
            string token = authorizationHeader.Substring("bearer".Length).Trim();
 
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized | Auth token not supplied");
            }
 
            try
            {
                return validateToken(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
 
        private AuthenticateResult validateToken(string token)
        {
            AuthToken authToken;
            if(tokenManager.IsTokenValid(token, out authToken) == false)
                return AuthenticateResult.Fail("Unauthorized | Invalid auth token supplied");

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, authToken.Token),
                };
 
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);

        }
    }


}