using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    class AddFeedResponse
    {
        [JsonProperty("feed")]
        public FeedResult Feed { get; set; }

        [JsonProperty("message")]
        public string Error { get; set; }
    }
}
