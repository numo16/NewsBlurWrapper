namespace Ayls.NewsBlur.Results
{
    public class MarkStoryAsUnreadResult : ApiCallResult
    {
        public bool IsMarkedAsUnread { get; private set; }

        public MarkStoryAsUnreadResult(bool isMarkedAsUnread)
        {
            IsMarkedAsUnread = isMarkedAsUnread;
            Status = ApiCallStatus.Ok;
        }

        public MarkStoryAsUnreadResult(string error, ApiCallStatus status)
            : base(error, status)
        {
            
        }
    }
}
