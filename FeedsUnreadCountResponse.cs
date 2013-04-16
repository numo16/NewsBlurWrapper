using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur
{
    class FeedsUnreadCountResponse
    {
        [JsonProperty("feeds")]
        public JObject Feeds { get; set; }
    }
}
