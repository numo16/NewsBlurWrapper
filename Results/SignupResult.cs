namespace Ayls.NewsBlur.Results
{
    public class SignupResult : ApiCallResult
    {
        public bool IsAuthenticated { get; set; }

        public SignupResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Status = ApiCallStatus.Ok;
        }

        public SignupResult(string error, ApiCallStatus status) : base (error, status)
        {
        }
    }
}
