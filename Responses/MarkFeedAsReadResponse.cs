using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkFeedAsReadResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool IsMarkedAsRead
        {
            get { return Code == 1; }
        }
    }
}
