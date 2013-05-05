using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkStoryAsUnstarredResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool IsUnstarred
        {
            get { return Code == 1; }
        }
    }
}
