namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsReadResult : ApiCallResult
    {
        public MarkStoryAsReadResult()
        {
            Status = ApiCallStatus.Ok;
        }

        public MarkStoryAsReadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
