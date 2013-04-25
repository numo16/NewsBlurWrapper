using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class LoginResponse
    {
        [JsonProperty("authenticated")]
        public bool IsAuthenticated { get; set; }
    }
}
