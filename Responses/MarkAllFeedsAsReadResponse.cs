using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkAllFeedsAsReadResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool AreMarkedAsRead
        {
            get { return Code == 1; }
        }
    }
}
