




using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Unit.Helpers
{


    public static class HttpHelperExtensions
    {

        public static IDictionary<string, string> WithHeader(this IDictionary<string, string> d, string key, string value) {

            d.Add(key, value);
            return d;            

        }

        public static void AddHeaders(this HttpRequestMessage request, IDictionary<string, string> headers) {

            if(headers != null && headers.Any())  {

                foreach(var kvp in headers) {

                    switch (kvp.Key) {

                        case "Authorization":
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", headers["Authorization"]);
                            break;

                        default:
                            request.Headers.Add(kvp.Key, new[] {kvp.Value});
                            break;

                    }
                }

            }
                
        }


    }

}