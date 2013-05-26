namespace Ayls.NewsBlur.Results
{
    public class MarkAllFeedsAsReadResult : ApiCallResult
    {
        internal MarkAllFeedsAsReadResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkAllFeedsAsReadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
