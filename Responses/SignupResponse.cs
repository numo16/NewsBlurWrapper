using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur.Responses
{
    class SignupResponse
    {
        [JsonProperty("authenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("errors")]
        public JToken Errors { get; set; }
    }
}
