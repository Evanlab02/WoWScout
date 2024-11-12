using Microsoft.Extensions.Options;

namespace Backend.Config
{
    /// <summary>
    /// Configuration class for the AuthClient.
    /// </summary>
    public class BlizzardAuthConfig
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }   
    }
}
