using Newtonsoft.Json;

namespace Ayls.NewsBlur
{
    class MarkStoryAsUnreadResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Error { get; set; }

        public bool IsSuccess
        {
            get { return Code == 0; }
        }
    }
}
