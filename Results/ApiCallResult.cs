using System.Collections.Generic;

namespace Ayls.NewsBlur.Results
{
    public abstract class ApiCallResult
    {
        public ApiCallStatus Status { get; internal set; }
        public List<string> Errors { get; internal set; }

        protected ApiCallResult()
        {
            Errors = new List<string>();
        }

        protected ApiCallResult(string error, ApiCallStatus status) : this()
        {
            Errors.Add(error);
            Status = status;
        }
    }
}
