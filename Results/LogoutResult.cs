namespace Ayls.NewsBlur.Results
{
    public class LogoutResult : ApiCallResult
    {
        internal LogoutResult()
        {
            Status = ApiCallStatus.Ok;
        }

        internal LogoutResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
