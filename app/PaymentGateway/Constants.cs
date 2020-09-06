



using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway
{

    public static class Constants
    {
        public const string ApplicationName = "PaymetGateway";

        public const string DefaultApiVersionAsString = "1.0";
        public static readonly ApiVersion DefaultApiVersion = new ApiVersion(1,0);
    }

}