using System.Text.Json.Serialization;

namespace Backend.Schemas
{
    /// <summary>
    /// Schema for the Blizzard tokens.
    /// </summary>
    /// <param name="accessToken">The OAauth token you would pass in the request.</param>
    /// <param name="expiresIn">The UNIX time in which the token expires.</param>
    /// <param name="sub">Bit of an unknown, but I believe this is a unique idenitifier to the Blizzard account this is setup with.</param>
    /// <param name="tokenType">The type of token we are receiving, very likely 'bearer'.</param>
    public class BlizzardAuthToken(string accessToken, string tokenType, int expiresIn, string sub)
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = accessToken;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = tokenType;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } = expiresIn;

        [JsonPropertyName("sub")]
        public string Sub { get; set; } = sub;
    }
}
