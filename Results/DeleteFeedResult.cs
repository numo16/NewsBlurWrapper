namespace Ayls.NewsBlur.Results
{
    public class DeleteFeedResult : ApiCallResult
    {
        internal DeleteFeedResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal DeleteFeedResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
