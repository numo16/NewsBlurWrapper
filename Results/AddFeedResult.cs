using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class AddFeedResult : ApiCallResult
    {
        public FeedSummaryResult Feed { get; private set; }

        internal AddFeedResult(FeedSummaryResponse feed, string folder)
        {
            Feed = new FeedSummaryResult(feed);
            Feed.Group = folder;
            Status = ApiCallStatus.Ok;
        }

        internal AddFeedResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}