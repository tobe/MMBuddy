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
using System.Web;

namespace MMBuddy.Services
{
    /// <summary>
    /// https://stackoverflow.com/a/39450277
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            return client.PatchAsync(CreateUri(requestUri), content);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            return client.PatchAsync(requestUri, content, CancellationToken.None);
        }
        /// <summary>
        /// Send a PATCH request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return client.PatchAsync(CreateUri(requestUri), content, cancellationToken);
        }

        /// <summary>
        /// Send a PATCH request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            }, cancellationToken);
        }

        private static Uri CreateUri(string uri)
        {
            return string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }

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

        /// <summary>
        /// Initializes the API client
        /// </summary>
        /// <param name="ServerInfo">The ServerInfo struct containing server information</param>
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

        public static async void SendChatMessage(string ChatRoomName, string Message)
        {
            // Parse the chat room name
            ChatRoomName = ChatRoomName.Split('@')[0].ToLower();
            ChatRoomName = HttpUtility.UrlEncode(ChatRoomName); // + & - !

            while (true)
            {
                // Keep sending POST until it succeeds. This is because
                // the chat room gets created after the session.
                HttpResponseMessage response = await _httpClient.PostAsync(
                    $"/lol-chat/v1/conversations/{ChatRoomName}/messages",
                    new
                    {
                        body = Message
                    }.AsJson()
                );

                if (response.IsSuccessStatusCode)
                    break;

                await Task.Delay(100);
            }

            return;
        }

        /// <summary>
        /// Hovers a champion
        /// </summary>
        /// <param name="ChampionId">The champion's id to hover</param>
        /// <param name="Completed">Complete the hover</param>
        /// <returns></returns>
        public static async Task<bool> Hover(int? PlayerId, int ChampionId, bool Completed)
        {
            var body = new
            {
                championId = ChampionId,
                completed = Completed
            };

            HttpResponseMessage response = await _httpClient.PatchAsync(
                $"/lol-champ-select/v1/session/actions/{PlayerId}",
                body.AsJson());

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the current matchmaking session if it exists
        /// </summary>
        /// <returns>The current matchmaking session, otherwise null</returns>
        public static async Task<Session> GetSession()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/lol-champ-select/v1/session");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Session>(
                    await response.Content.ReadAsStringAsync());
            }

            return null;
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
