namespace Ayls.NewsBlur
{
    public class AddFeedResult
    {
        public FeedResult Feed { get; set; }
        public string Error { get; set; }
        public bool IsSuccess
        {
            get { return Feed != null; }
        }
    }
}