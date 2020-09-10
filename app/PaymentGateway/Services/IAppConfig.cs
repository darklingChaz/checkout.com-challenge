


using System;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Services {


    public interface IAppConfig {

        TimeSpan TokenExpiry {get;}
        string BankingApiEndpoint {get;}
    }

    public class AppConfig : IAppConfig {
        private readonly IConfiguration configuration;

        public AppConfig(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TimeSpan TokenExpiry {

            get {
                var t = configuration.GetValue<int>("TokenExpiryInSeconds");
                return t == 0 ? Constants.DefaultTokenExpiry : TimeSpan.FromSeconds(t);
            }

        }

        public string BankingApiEndpoint => configuration.GetValue<string>("BankingApiEndpoint");
    }


}
