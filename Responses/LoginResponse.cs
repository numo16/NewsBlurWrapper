using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur.Responses
{
    class LoginResponse
    {
        [JsonProperty("authenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errors")]
        public JToken Errors { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

    }
}
