using MMBuddy.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMBuddy.Services
{
    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Request:");
            Debug.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Debug.WriteLine(await request.Content.ReadAsStringAsync());
            }
            Debug.WriteLine("");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Debug.WriteLine("Response:");
            Debug.WriteLine(response.ToString());
            if (response.Content != null)
            {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
            }
            Debug.WriteLine("");

            return response;
        }
    }

    static class ApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

        public static void Initialize(ServerInfo ServerInfo)
        {
            // Set up the HTTP Client
            _httpClient.BaseAddress = new Uri("https://127.0.0.1:" + ServerInfo.Port);

            // Accept JSON
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Setup Auth
            var byteArray = Encoding.Default.GetBytes("riot:" + ServerInfo.Token);
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Ignore certs
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Returns the current rune page from the client.
        /// </summary>
        public static async Task<RunePage> GetCurrentRunePage()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/lol-perks/v1/currentpage");
            if(response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<RunePage>(
                    await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        /// <summary>
        /// Sets a currently selected clientside runepage to a serverside runepage.
        /// </summary>
        /// <param name="RunePage">The RunePage to set</param>
        /// <returns></returns>
        public static async Task<bool> SetCurrentRunePage(RunePage RunePage)
        {
            // First get the current rune page
            var currentRunePage = await GetCurrentRunePage();
            if (currentRunePage == null) return false;

            // Check if reserved --> Can't swap stock pages
            if (currentRunePage.Id >= 50 && currentRunePage.Id <= 54)
                return false;

            // Now put the new rune page in place of currently selected one
            HttpResponseMessage response = await _httpClient.PutAsync(
                $"/lol-perks/v1/pages/{currentRunePage.Id}",
                RunePage.AsJson()
            );
            response.EnsureSuccessStatusCode();

            return true;
        }


        #region Helpers

        /// <summary>
        /// https://stackoverflow.com/a/44937617
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static StringContent AsJson(this object o)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return new StringContent(JsonConvert.SerializeObject(o, serializerSettings),
                Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
