using Backend.Config;
using Backend.Schemas;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace Backend.Clients
{
    /// <summary>
    /// The client for retrieving authentication tokens and values from the blizzard services.
    /// </summary>
    public class AuthClient
    {
        private readonly ILogger<AuthClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _tokenEndpoint = "https://oauth.battle.net/token";
        private readonly string _clientId;
        private readonly string _clientSecret;

        /// <summary>
        /// Constructor for the AuthClient.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClient">The client we will be using to send requests.</param>
        /// <param name="configuration">The configuration containing clientId and clientSecret.</param>
        public AuthClient(ILogger<AuthClient> logger, HttpClient httpClient, BlizzardAuthConfig configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _clientId = configuration.ClientId;
            _clientSecret = configuration.ClientSecret;
        }

        /// <summary>
        /// Retrieves a Blizzard API oauth token.
        /// </summary>
        /// <returns>An authentication token as an <see cref="BlizzardAuthToken"/> object.</returns>
        public async Task<BlizzardAuthToken> GetAuthToken()
        {
            // Set the request URL.
            string requestUrl = _tokenEndpoint;
            _logger.LogInformation("Retrieving blizzard oauth token.");

            // Set the credentials in the "Authorization" header
            var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Set up the form data
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // Send the POST request
            var response = await _httpClient.PostAsync(requestUrl, content);

            // Check if the response was successful
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching auth token: {response.ReasonPhrase}");
            }

            // Deserialize the response content into AuthTokenResponse
            var responseContent = await response.Content.ReadAsStringAsync();
            var authTokenResponse = JsonSerializer.Deserialize<BlizzardAuthToken>(responseContent);

            if (authTokenResponse == null)
            {
                throw new SerializationException("Error converting auth token.");
            }

            return authTokenResponse;
        }
    }
}
