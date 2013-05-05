using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkStoryAsReadResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errors")]
        public IEnumerable<string> Errors { get; set; }

        public bool IsRead
        {
            get { return Code == 0; }
        }
    }
}
