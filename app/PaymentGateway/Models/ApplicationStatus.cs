



using System;
using System.Diagnostics;

namespace PaymentGateway.Controllers {



    public class ApplicationStatus {
        public string ApplicationName { get; }
        public long TimeAliveInMilliseconds { get; }

        public ApplicationStatus()
        {
            ApplicationName = Constants.ApplicationName;
            TimeAliveInMilliseconds = (long)DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds;
        }
    }

}