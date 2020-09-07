




using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Unit.BDD {


    [TestFixture]
    public class ApplicationSmokeTests {


        private PaymentGatewayHelper paymentGatewayHelper;


        [SetUp] 
        public void Setup() {

            paymentGatewayHelper = new PaymentGatewayHelper();

        }

        [TearDown]
        public async Task Teardown() {

            await paymentGatewayHelper.StopGateway();
        }


        [Test]
        public void ApplicationNotRunning_QueryInfo_ThrowsException() {
        
            // Given
        
            // When
        
            // Then
            Assert.ThrowsAsync<HttpRequestException>(() => paymentGatewayHelper.GetApplicationStatus());
        
        }

        [Test]
        public async Task ApplicationIsUp_QueryInfo_Ok() {
        
            // Given
            await paymentGatewayHelper.StartGateway();

            // When
            var info = await paymentGatewayHelper.GetApplicationStatus();
        
            // Then
            Assert.AreEqual("PaymentGateway", info.ApplicationName);
            Assert.That(info.TimeAliveInMilliseconds, Is.GreaterThan(0));
        
        }



    }



}