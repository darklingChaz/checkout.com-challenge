


using System;
using System.Collections.Generic;
using System.Linq;
using PaymentGateway.Models.Auth;

namespace PaymentGateway.Auth
{



    public interface ICredentialTokenManager
    {
        AuthToken Authenticate(AuthCredentials creds);

        bool IsTokenValid(string token, out AuthToken authToken);
    }

    public class CredentialTokenManager : ICredentialTokenManager
    {

        private static readonly object lck = new object();

        private readonly IDictionary<AuthCredentials, AuthToken> tokenCache = new Dictionary<AuthCredentials, AuthToken>();

        private readonly HashSet<AuthCredentials> validCreds;
        private readonly TimeSpan tokenExpiry;


        public CredentialTokenManager(TimeSpan tokenExpiry, params AuthCredentials[] validCreds)
        {
            this.validCreds = new HashSet<AuthCredentials>(validCreds);
            this.tokenExpiry = tokenExpiry;
        }



        public AuthToken Authenticate(AuthCredentials creds)
        {
            if (!validCreds.Any(u => u.Equals(creds)))
            {
                return null;
            }

            var token = GetOrAddToken(creds);
            return token;
        }

        private AuthToken GetOrAddToken(AuthCredentials creds)
        {
            lock (lck)
            {
                if (tokenCache.ContainsKey(creds))
                {
                    if (tokenCache[creds].HasExpired)
                    {
                        tokenCache.Remove(creds);
                    }
                    else
                        return tokenCache[creds];
                }

                var token = AuthToken.CreateNew(tokenExpiry);
                tokenCache.Add(creds, token);
                return token;
            }
        }

        public bool IsTokenValid(string token, out AuthToken authToken)
        {
            lock(lck) {

                authToken = tokenCache.Values.SingleOrDefault(at => at.Token == token);
                if(authToken != null) {
                    if(authToken.HasExpired)
                    {
                        authToken = null;
                        return false;
                    }

                    return true;
                }

                return false;
            }
        }
    }



}