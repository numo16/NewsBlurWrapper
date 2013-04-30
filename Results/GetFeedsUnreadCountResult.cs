using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetFeedsUnreadCountResult : ApiCallResult
    {
        public IEnumerable<FeedUnreadCountSummaryResult> Feeds { get; private set; }

        internal GetFeedsUnreadCountResult(IEnumerable<FeedUnreadCountSummaryResult> feeds)
        {
            Feeds = feeds;
            Status = ApiCallStatus.Ok;
        }

        internal GetFeedsUnreadCountResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
