using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetGroupedFeedsResult : ApiCallResult
    {
        public IEnumerable<FeedSummaryResult> Feeds { get; private set; }

        public GetGroupedFeedsResult(IEnumerable<FeedSummaryResult> feeds)
        {
            Feeds = feeds;
            Status = ApiCallStatus.Ok;
        }

        public GetGroupedFeedsResult(string error, ApiCallStatus status) : base(error, status)
        {
        }
    }
}
