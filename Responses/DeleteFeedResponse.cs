using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class DeleteFeedResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool IsDeleted
        {
            get { return Code == 1; }
        }
    }
}
