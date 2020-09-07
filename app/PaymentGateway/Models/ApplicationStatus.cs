



using System;
using System.Diagnostics;

namespace PaymentGateway.Models {



    public class ApplicationStatus {

        public string ApplicationName { get; set; }
        public long TimeAliveInMilliseconds { get; set; }

        public static ApplicationStatus Get()
        {
            return new ApplicationStatus {
                ApplicationName = Constants.ApplicationName,
                TimeAliveInMilliseconds = (long)DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds
            };
        }
    }

}