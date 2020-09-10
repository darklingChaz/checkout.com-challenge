





using System;
using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.Auth;
using PaymentGateway.Models.Auth;

namespace Unit {


    [TestFixture]
    public class CredentialTokenManagerTests
    {
        

        private static readonly AuthCredentials[] validCreds = {
            
            new AuthCredentials("User1", "Pwd1"),
            new AuthCredentials("User2", "Pwd2"),

        };

        private static readonly TimeSpan tokenExpiry = TimeSpan.FromMilliseconds(5);


        private CredentialTokenManager credentialTokenManager;


        [SetUp]
        public void Setup() {

            credentialTokenManager = new CredentialTokenManager(tokenExpiry, validCreds);

        }




        [Test]
        public void ValidUser_CanGetAuthenticationToken() {
        
            // Given
            var creds = validCreds[0];

            // When
            var authToken = credentialTokenManager.Authenticate(creds);
        
            // Then
            Assert.IsNotNull(authToken);
            Assert.IsNotNull(authToken.Token);
            Assert.That(authToken.ExpiresUtc, Is.Not.GreaterThan(DateTime.UtcNow.Add(tokenExpiry)));
        }


        [Test]
        public void InvalidUser_ReturnsNull() {
        
            // Given
            var invalidCreds = new AuthCredentials("UNKNOWN", "NOT POPULATED");
        
            // When
            var authToken = credentialTokenManager.Authenticate(invalidCreds);
        
            // Then
            Assert.IsNull(authToken);
        
        }

        [Test]
        public void RepeatAuthenticateReturnsSameToken_WhenNotExpired() {
        
            // Given
            var creds = validCreds[0];

            // When
            var at1 = credentialTokenManager.Authenticate(creds);
            var at2 = credentialTokenManager.Authenticate(creds);
            var at3 = credentialTokenManager.Authenticate(creds);
        
            // Then
            Assert.AreEqual(at1, at2);
            Assert.AreEqual(at1, at3);
        
        }

        [Test]
        public async Task IfTimeBetweenAuthsIsGreaterThanExpiry_GeneratesNewToken() {
        
            // Given
            var creds = validCreds[0];
        
            // When
            var at1 = credentialTokenManager.Authenticate(creds);
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            var at2 = credentialTokenManager.Authenticate(creds);
        
            // Then
            Assert.AreNotEqual(at1, at2);
        
        }


        [Test]
        public void HaveInvalidToken_CheckIsTokenValidReturnFalse() {
        
            // Given
            var invalidToken = new AuthToken {
                Token = "UNKNOWN"
            };
        
            // When
            var result = credentialTokenManager.IsTokenValid(invalidToken.Token, out var _);
        
            // Then
            Assert.IsFalse(result);
        
        }

        [Test]
        public async Task HaveExpiredToken_CheckIsTokenValidReturnFalse() {
        
            // Given
            var creds = validCreds[0];
        
            // When
            var authToken = credentialTokenManager.Authenticate(creds);
            await Task.Delay(TimeSpan.FromMilliseconds(50));

            var result = credentialTokenManager.IsTokenValid(authToken.Token, out var _);
        
            // Then
            Assert.IsFalse(result);
        
        }

        [Test]
        public void HaveValidToken_CheckIsTokenValidReturnsTrue() {
        
            // Given
            var creds = validCreds[0];
        
            // When
            var authToken = credentialTokenManager.Authenticate(creds);

            AuthToken outToken;
            var result = credentialTokenManager.IsTokenValid(authToken.Token, out outToken);
        
            // Then
            Assert.IsTrue(result);
            Assert.AreEqual(authToken, outToken);
        
        }

    }


}