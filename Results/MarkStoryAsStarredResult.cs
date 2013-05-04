namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsStarredResult : ApiCallResult
    {
        internal MarkStoryAsStarredResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkStoryAsStarredResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
