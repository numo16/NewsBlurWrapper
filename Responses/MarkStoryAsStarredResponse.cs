using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkStoryAsStarredResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool IsMarkedAsStarred
        {
            get { return Code == 1; }
        }
    }
}
