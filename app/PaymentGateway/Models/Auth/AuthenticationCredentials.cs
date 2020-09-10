




using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Models.Auth
{

    [SwaggerTag("Basic username/password auth credentials")]
    public class AuthCredentials
    {

        [Required]
        public string Username { get; set; }
        [Required]

        public string Password { get; set; }


        public AuthCredentials() { }

        public AuthCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            return obj is AuthCredentials credentials &&
                   Username == credentials.Username &&
                   Password == credentials.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Password);
        }
    }

}