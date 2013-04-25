using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur.Responses
{
    class FeedsUnreadCountResponse
    {
        [JsonProperty("feeds")]
        public JObject Feeds { get; set; }
    }
}
