namespace Ayls.NewsBlur.Results
{
    public class MarkFeedAsReadResult : ApiCallResult
    {
        internal MarkFeedAsReadResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkFeedAsReadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
