using System.Collections.Generic;
using System.Linq;
using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class GetStoriesResult : ApiCallResult
    {
        public IEnumerable<StorySummaryResult> Stories { get; private set; }

        internal GetStoriesResult(IEnumerable<StorySummaryResponse> stories)
        {
            Stories = stories.Select(x => new StorySummaryResult(x));
            Status = ApiCallStatus.Ok;
        }

        internal GetStoriesResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
