using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur
{
    class SignupResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("authenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("errors")]
        public JObject Errors { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
