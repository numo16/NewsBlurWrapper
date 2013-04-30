using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class StoriesResponse
    {
        [JsonProperty("stories")]
        public IEnumerable<StorySummaryResponse> Stories { get; set; }
    }
}
