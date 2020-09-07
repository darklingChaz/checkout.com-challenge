

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Unit.Helpers
{


    public abstract class ApiQueryHelper
    {

        public readonly string BaseUrl;

        private readonly HttpClient client;



        protected ApiQueryHelper(string baseUrl)
        {
            BaseUrl = baseUrl;
            client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }


        public async Task<string> ExecuteHttpRequestReturnJson(HttpMethod method, string path)
        {
            return await ExecuteHttpRequestActualReturnString(method, path);
        }

        public async Task<T> ExecuteHttpRequest<T>(HttpMethod method, string path)
        {
            var responseJson = await ExecuteHttpRequestActualReturnString(method, path);
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        public async Task<HttpResponseMessage> ExecuteHttpRequest(HttpMethod method, string path)
        {
            return await ExecuteHttpRequestActual(method, path);
        }


        public async Task<string> ExecuteHttpRequestReturnJson<TIn>(HttpMethod method, string path, TIn body)
            where TIn : class
        {
            var requestJson = JsonConvert.SerializeObject(body);
            return await ExecuteHttpRequestActualReturnString(method, path, requestJson);
        }

        public async Task<TOut> ExecuteHttpRequest<TIn, TOut>(HttpMethod method, string path, TIn body)
            where TIn : class
            where TOut : class
        {
            var responseJson = await ExecuteHttpRequestReturnJson<TIn>(method, path, body);
            return JsonConvert.DeserializeObject<TOut>(responseJson);
        }

        public async Task<HttpResponseMessage> ExecuteHttpRequest<T>(HttpMethod method, string path, T body, IDictionary<string, string> headers = null)
            where T : class
        {
            var requestJson = JsonConvert.SerializeObject(body);
            return await ExecuteHttpRequestActual(method, path, requestJson, headers);
        }


        private async Task<string> ExecuteHttpRequestActualReturnString(HttpMethod method, string path, string content = null)
        {

            using var response = await ExecuteHttpRequestActual(method, path, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GOT RESULT | {BaseUrl}{path} | {body}");
                return body;
            }

            // Would expect an error above before getting here.
            return null;

        }

        private async Task<HttpResponseMessage> ExecuteHttpRequestActual(HttpMethod method, string path, string content = null, IDictionary<string, string> headers = null)
        {

            var request = new HttpRequestMessage(method, path);

            Console.WriteLine($"HITTING URL | {BaseUrl}{path}");

            if (string.IsNullOrWhiteSpace(content) == false)
            {
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
                request.Content = requestContent;
            }

            if (headers != null && headers.Any())
            {
                if (headers.ContainsKey("Authorization"))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", headers["Authorization"]);
            }

            return await client.SendAsync(request);
        }

    }

}