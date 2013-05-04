using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ayls.NewsBlur.Responses
{
    class FeedsResponse
    {
        [JsonProperty("folders")]
        public IEnumerable<JToken> Groups { get; set; }

        [JsonProperty("feeds")]
        public JToken Feeds { get; set; }
    }
}
