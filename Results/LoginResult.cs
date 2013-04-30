namespace Ayls.NewsBlur.Results
{
    public class LoginResult : ApiCallResult
    {
        public bool IsAuthenticated { get; private set; }

        internal LoginResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Status = ApiCallStatus.Ok;
        }

        internal LoginResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }
    }
}
