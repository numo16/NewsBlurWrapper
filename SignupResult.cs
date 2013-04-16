using System.Collections.Generic;

namespace Ayls.NewsBlur
{
    public class SignupResult
    {
        public bool IsAuthenticated { get; set; }

        public List<string> Errors { get; set; }
    }
}
