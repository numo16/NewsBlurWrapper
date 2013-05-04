using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsReadResult : ApiCallResult
    {
        internal MarkStoryAsReadResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkStoryAsReadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }

        internal MarkStoryAsReadResult(IEnumerable<string> errors, ApiCallStatus status)
            : base(errors, status)
        {
        }
    }
}
