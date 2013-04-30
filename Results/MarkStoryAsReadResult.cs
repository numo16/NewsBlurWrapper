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
    }
}
