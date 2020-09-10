



using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Controllers {


    [ApiController]
    [Route("")]
    [Produces("application/json")]
    [SwaggerTag("Status of the service")]
    public class InfoController : ControllerBase    
    {


        [HttpGet("/info")] 
        [SwaggerOperation("Info", "Get info of service")]
        [SwaggerResponse(200, "OK", typeof(ApplicationStatus))]
        [SwaggerResponse(500, "Internal Server Error", typeof(ProblemDetails))]
        public ApplicationStatus Info() {
            return ApplicationStatus.Get();
        }


    }



}