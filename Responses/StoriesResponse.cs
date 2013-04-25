using System.Collections.Generic;
using Ayls.NewsBlur.Results;
using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class StoriesResponse
    {
        [JsonProperty("stories")]
        public IEnumerable<StorySummaryResult> Stories { get; set; }
    }
}
