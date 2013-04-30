using Ayls.NewsBlur.Responses;

namespace Ayls.NewsBlur.Results
{
    public class FeedSummaryResult
    {
        internal FeedSummaryResult(FeedSummaryResponse response)
        {
            Id = response.Id;
            Active = response.Active;
            Link = response.Link;
            Title = response.Title;
        }

        public string Id { get; set; }
        public bool Active { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
    }
}
