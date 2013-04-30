using System;
using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class StorySummaryResult
    {
        internal StorySummaryResult(StorySummaryResponse response)
        {
            Id = response.Id;
            Link = response.Link;
            Title = response.Title;
            IsRead = response.IsRead;
            Timestamp = response.Timestamp;
        }

        public string Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public DateTime Timestamp { get; set; }
    }
}