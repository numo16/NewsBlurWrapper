using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class FeedUnreadCountSummaryResult
    {
        internal FeedUnreadCountSummaryResult(FeedUnreadCountSummaryResponse response)
        {
            Id = response.Id;
            UnreadCount = response.UnreadCount;
            InFocusCount = response.InFocusCount;
        }

        public string Id { get; set; }
        public int UnreadCount { get; set; }
        public int InFocusCount { get; set; }
    }
}
