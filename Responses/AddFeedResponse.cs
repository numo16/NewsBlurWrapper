using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class AddFeedResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("feed")]
        public FeedSummaryResponse Feed { get; set; }

        [JsonProperty("message")]
        public string Error { get; set; }

        public bool IsFeedAdded
        {
            get { return Code == 1; }
        }
    }
}
