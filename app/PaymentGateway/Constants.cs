



using System;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway
{

    public static class Constants
    {
        public const string ApplicationName = "PaymentGateway";

        public const string DefaultApiVersionAsString = "1.0";
        public static readonly ApiVersion DefaultApiVersion = new ApiVersion(1,0);
        public static readonly TimeSpan DefaultTokenExpiry = TimeSpan.FromMinutes(10);
    }

}