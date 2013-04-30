using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class FeedSummaryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("feed_link")]
        public string Link { get; set; }

        [JsonProperty("feed_title")]
        public string Title { get; set; }
    }
}
