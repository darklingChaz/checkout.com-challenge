



using System;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Models.Auth
{

    [SwaggerTag("Auth token response with expiry")]
    public class AuthToken
    {

        public string Token { get; set; }
        public DateTime ExpiresUtc { get; set; }


        [JsonIgnore]
        public bool HasExpired => ExpiresUtc < DateTime.UtcNow;


        public AuthToken() { }

        public static AuthToken CreateNew(TimeSpan expiry)
        {
            return new AuthToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiresUtc = DateTime.UtcNow.Add(expiry)
            };
        }

        public override string ToString()
        {
            return $"Token = {Token} | ExpiresUtc = {ExpiresUtc}";
        }

    }

}