using Backend.Schemas;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/blizzard/token")]
    public class AuthTokenController : Controller
    {
        private readonly ILogger<AuthTokenController> _logger;
        private readonly AuthTokenService _authTokenService;

        public AuthTokenController(ILogger<AuthTokenController> logger, AuthTokenService authTokenService)
        {
            _logger = logger;
            _authTokenService = authTokenService;
        }

        [HttpGet(Name = "")]
        [ProducesResponseType(typeof(BlizzardAuthToken), 200)]
        public async Task<IResult> Token()
        {
            _logger.LogInformation("Received request to obtain auth token.");
            return await _authTokenService.GetTokenAsync();
        }
    }
}
