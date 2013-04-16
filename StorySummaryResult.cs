using System;
using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    public class StorySummaryResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("story_permalink")]
        public string Url { get; set; }

        [JsonProperty("story_title")]
        public string Title { get; set; }

        [JsonProperty("read_status")]
        public bool IsRead { get; set; }

        [JsonProperty("story_date")]
        public DateTime Timestamp { get; set; }
    }
}