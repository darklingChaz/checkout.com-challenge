



using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Controllers {



    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase {


        [HttpGet]
        [MapToApiVersion("1.0")]
        public string IdVersion1() {
            return "IdVersion1";
        }


        [HttpGet]
        [MapToApiVersion("2.0")]
        public string IdVersion2() {
            return "IdVersion2";
        }
    }




}