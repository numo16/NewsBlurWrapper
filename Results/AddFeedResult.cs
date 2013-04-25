namespace Ayls.NewsBlur.Results
{
    public class AddFeedResult : ApiCallResult
    {
        public FeedSummaryResult Feed { get; private set; }
        public bool IsFeedAdded { get; private set; }

        public AddFeedResult(FeedSummaryResult feed, bool isFeedAdded)
        {
            Feed = feed;
            IsFeedAdded = isFeedAdded;
            Status = ApiCallStatus.Ok;
        }

        public AddFeedResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}