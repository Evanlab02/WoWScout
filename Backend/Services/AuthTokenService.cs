using Backend.Clients;
using Backend.Schemas;
using System.Runtime.Serialization;

namespace Backend.Services
{
    public class AuthTokenService
    {
        private readonly ILogger<AuthClient> _logger;
        private readonly AuthClient _client;

        // Constructor for dependency injection
        public AuthTokenService(ILogger<AuthClient> logger, AuthClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IResult> GetTokenAsync()
        {
            try
            {
                BlizzardAuthToken token = await _client.GetAuthToken();
                return Results.Ok(token);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString());
                return Results.Problem(ex.ToString());
            }
            catch (SerializationException ex)
            {
                _logger.LogError(ex.ToString());
                return Results.Problem(ex.ToString());
            }
        }
    }
}
