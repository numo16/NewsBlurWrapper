using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetGroupedFeedsResult : ApiCallResult
    {
        public IEnumerable<FeedSummaryResult> Feeds { get; private set; }

        internal GetGroupedFeedsResult(IEnumerable<FeedSummaryResult> feeds)
        {
            Feeds = feeds;
            Status = ApiCallStatus.Ok;
        }

        internal GetGroupedFeedsResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
