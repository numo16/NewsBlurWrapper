using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    public class FeedUnreadCountResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nt")]
        public int UnreadCount { get; set; }
    }
}
