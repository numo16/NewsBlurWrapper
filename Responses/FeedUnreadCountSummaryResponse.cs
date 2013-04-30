using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class FeedUnreadCountSummaryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nt")]
        public int UnreadCount { get; set; }
    }
}
