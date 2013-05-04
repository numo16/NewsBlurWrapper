using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class StorySummaryIntelligenceResponse
    {
        [JsonProperty("feed")]
        public int FeedCount { get; set; }

        [JsonProperty("tags")]
        public int TagCount { get; set; }

        [JsonProperty("author")]
        public int AuthorCount { get; set; }

        [JsonProperty("title")]
        public int TitleCount { get; set; }
    }
}
