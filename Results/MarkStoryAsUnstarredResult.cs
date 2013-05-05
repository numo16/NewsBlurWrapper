namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsUnstarredResult : ApiCallResult
    {
        internal MarkStoryAsUnstarredResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkStoryAsUnstarredResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
