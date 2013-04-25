namespace Ayls.NewsBlur.Results
{
    public class LogoutResult : ApiCallResult
    {
        public LogoutResult()
        {
            Status = ApiCallStatus.Ok;
        }

        public LogoutResult(string error, ApiCallStatus status) : base (error, status)
        {
        }
    }
}
