using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class FeedUnreadCountSummaryResult
    {
        internal FeedUnreadCountSummaryResult(FeedUnreadCountSummaryResponse response)
        {
            Id = response.Id;
            UnreadCount = response.UnreadCount;
        }

        public string Id { get; set; }
        public int UnreadCount { get; set; }
    }
}
