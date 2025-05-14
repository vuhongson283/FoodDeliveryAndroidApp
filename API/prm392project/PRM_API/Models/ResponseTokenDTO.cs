using Newtonsoft.Json;

namespace PRM_API.Models
{
    public class ResponseTokenDTO
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public ResponseTokenDTO(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
