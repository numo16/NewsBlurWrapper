using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkStoryAsUnreadResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Error { get; set; }

        public bool IsUnread
        {
            get { return Code == 0; }
        }
    }
}
