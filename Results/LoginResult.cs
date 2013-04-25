namespace Ayls.NewsBlur.Results
{
    public class LoginResult : ApiCallResult
    {
        public bool IsAuthenticated { get; private set; }

        public LoginResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Status = ApiCallStatus.Ok;
        }

        public LoginResult(string error, ApiCallStatus status) : base(error, status)
        {
        }
    }
}
