namespace Ayls.NewsBlur
{
    public class MarkStoryAsUnreadResult
    {
        public string Error { get; set; }
        public bool IsSuccess
        {
            get { return string.IsNullOrEmpty(Error); }
        }
    }
}