




using System;

namespace PaymentGateway.Models.Auth {


    public class AuthCredentials
    {

        public string Username { get; set; }
        public string Password { get; set; }
        

        public AuthCredentials() {}

        public AuthCredentials(string username, string password) {
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