using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    class LoginResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("authenticated")]
        public bool IsAuthenticated { get; set; }
    }
}
