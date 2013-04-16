﻿using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    public class FeedResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("feed_link")]
        public string Link { get; set; }

        [JsonProperty("feed_title")]
        public string Title { get; set; }

        public string Group { get; set; }
    }
}
