



using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Auth;
using PaymentGateway.Exceptions;
using PaymentGateway.Models.Auth;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Controllers
{



    [RequireHttps]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payment")]
    public class PaymentController : ControllerBase
    {

        private readonly ICredentialTokenManager credentialTokenManager;
        private readonly IPaymentProcessorService paymentProcessorService;


        public PaymentController(ICredentialTokenManager credentialTokenManager, IPaymentProcessorService paymentProcessorService)
        {
            this.credentialTokenManager = credentialTokenManager;
            this.paymentProcessorService = paymentProcessorService;
        }




        [AllowAnonymous]
        [HttpPost("authtoken")]
        [SwaggerOperation("Get auth token", "Get auth session token")]
        [SwaggerResponse(200, "OK", typeof(AuthToken))]
        [SwaggerResponse(401, "Unauthorized", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
        public IActionResult GetAuthToken([FromBody] AuthCredentials credentials)
        {

            var authToken = credentialTokenManager.Authenticate(credentials);
            if (authToken == null)
                return StatusCode(401); // NotAuthorized

            return Ok(authToken);

        }


        [Authorize]
        [HttpPost("submit")]
        [SwaggerOperation("Submit a payment", "Submit a payment")]
        [SwaggerOperationFilter(typeof(CustomTokenSwaggerOperationFilter))]
        [SwaggerResponse(200, "OK", typeof(BankTransactionResponse))]
        [SwaggerResponse(400, "BadRequest - PaymentDetails not valid", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
        public async Task<IActionResult> SubmitPayment([FromBody] PaymentDetails paymentDetails)
        {

            try
            {
                var transaction = await paymentProcessorService.Process(paymentDetails); // should maybe return a different status code on WasSuccess = false? But which code?
                return Ok(transaction);
            }
            catch (Exception e) when (e is PaymentDetailsInvalidException || e is CreditCardNumberInvalidException)
            {
                return StatusCode(400, e);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

        }


    }




}