namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsUnreadResult : ApiCallResult
    {
        internal MarkStoryAsUnreadResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal MarkStoryAsUnreadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
            
        }
    }
}
