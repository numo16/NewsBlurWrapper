﻿using Newtonsoft.Json;

namespace Ayls.NewsBlur.Responses
{
    class MarkStoryAsStarredResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        public bool IsStarred
        {
            get { return Code == 1; }
        }
    }
}
