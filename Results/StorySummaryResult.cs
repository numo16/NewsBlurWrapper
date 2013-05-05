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
            IsInFocus = response.Intelligence != null &&
                        (response.Intelligence.FeedCount > 0 || 
                         response.Intelligence.TagCount > 0 ||
                         response.Intelligence.AuthorCount > 0 || 
                         response.Intelligence.TitleCount > 0);
            FeedId = response.FeedId;
            IsStarred = response.IsStarred;
            Timestamp = response.Timestamp;
        }

        public string Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public bool IsInFocus { get; set; }
        public string FeedId { get; set; }
        public bool IsStarred { get; set; }
        public DateTime Timestamp { get; set; }
    }
}