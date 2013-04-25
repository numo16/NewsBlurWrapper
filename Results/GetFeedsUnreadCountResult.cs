using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetFeedsUnreadCountResult : ApiCallResult
    {
        public IEnumerable<FeedUnreadCountSummaryResult> Feeds { get; private set; }

        public GetFeedsUnreadCountResult(IEnumerable<FeedUnreadCountSummaryResult> feeds)
        {
            Feeds = feeds;
            Status = ApiCallStatus.Ok;
        }

        public GetFeedsUnreadCountResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
