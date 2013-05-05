using System;
using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class StorySummaryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("story_permalink")]
        public string Link { get; set; }

        [JsonProperty("story_title")]
        public string Title { get; set; }

        [JsonProperty("read_status")]
        public bool IsRead { get; set; }

        [JsonProperty("story_date")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("story_feed_id")]
        public string FeedId { get; set; }

        [JsonProperty("starred")]
        public bool IsStarred { get; set; }

        [JsonProperty("story_content")]
        public string Content { get; set; }

        [JsonProperty("intelligence")]
        public StorySummaryIntelligenceResponse Intelligence { get; set; }
    }
}