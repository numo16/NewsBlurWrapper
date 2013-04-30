namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsUnreadResult : ApiCallResult
    {
        public bool IsMarkedAsUnread { get; private set; }

        internal MarkStoryAsUnreadResult(bool isMarkedAsUnread)
        {
            IsMarkedAsUnread = isMarkedAsUnread;
            Status = ApiCallStatus.Ok;
        }

        internal MarkStoryAsUnreadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
            
        }
    }
}
