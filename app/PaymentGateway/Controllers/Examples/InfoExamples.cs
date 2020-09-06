


using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.Controllers.Examples {

    public class InfoExample : IExamplesProvider<ApplicationStatus>
    {
        public ApplicationStatus GetExamples() => new ApplicationStatus();
    }

}