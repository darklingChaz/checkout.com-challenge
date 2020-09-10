



using System;
using PaymentGateway.Models.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.Controllers.Examples
{



    public class AuthCredentialsExample : IExamplesProvider<AuthCredentials>
    {
        public AuthCredentials GetExamples()
        {
            return new AuthCredentials("UserName", "Password");
        }
    }


    public class AuthTokenExample : IExamplesProvider<AuthToken>
    {
        public AuthToken GetExamples()
        {
            return new AuthToken{ Token = Guid.Empty.ToString(), ExpiresUtc = DateTime.MinValue };
        }
    }

}