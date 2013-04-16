using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    class StoriesResponse
    {
        [JsonProperty("stories")]
        public IEnumerable<StorySummaryResult> Stories { get; set; }
    }
}
