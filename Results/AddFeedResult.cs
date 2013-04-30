using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class AddFeedResult : ApiCallResult
    {
        public FeedSummaryResult Feed { get; private set; }
        public bool IsFeedAdded { get; private set; }

        internal AddFeedResult(FeedSummaryResponse feed, bool isFeedAdded)
        {
            Feed = new FeedSummaryResult(feed);
            IsFeedAdded = isFeedAdded;
            Status = ApiCallStatus.Ok;
        }

        internal AddFeedResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}