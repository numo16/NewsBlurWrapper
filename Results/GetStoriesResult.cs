using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class GetStoriesResult : ApiCallResult
    {
        public IEnumerable<StorySummaryResult> Stories { get; private set; }

        public GetStoriesResult(IEnumerable<StorySummaryResult> feeds)
        {
            Stories = feeds;
            Status = ApiCallStatus.Ok;
        }

        public GetStoriesResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
