using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetGroupedFeedsResult : ApiCallResult
    {
        public int StarredCount { get; set; }

        public IEnumerable<FeedSummaryResult> Feeds { get; private set; }

        internal GetGroupedFeedsResult(int starredCount, IEnumerable<FeedSummaryResult> feeds)
        {
            StarredCount = starredCount;
            Feeds = feeds;
            Status = ApiCallStatus.Ok;
        }

        internal GetGroupedFeedsResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
