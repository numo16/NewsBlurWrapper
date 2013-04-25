using Newtonsoft.Json;

namespace Ayls.NewsBlur.Results
{
    public class FeedUnreadCountSummaryResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nt")]
        public int UnreadCount { get; set; }
    }
}
