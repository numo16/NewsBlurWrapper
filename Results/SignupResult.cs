using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public class SignupResult : ApiCallResult
    {
        public bool IsAuthenticated { get; set; }

        internal SignupResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Status = ApiCallStatus.Ok;
        }

        internal SignupResult(string error, ApiCallStatus status)
            : base(error, status)
        {
        }

        internal SignupResult(IEnumerable<string> errors, ApiCallStatus status)
            : base(errors, status)
        {
        }
    }
}
