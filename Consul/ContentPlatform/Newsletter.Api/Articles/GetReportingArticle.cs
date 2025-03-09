namespace Newsletter.Api.Articles;

public class GetReportingArticle
{
    public class Response
    {
        public Guid Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? PublishedOnUtc { get; set; }

        public List<ArticleEventResponse> Events { get; set; } = new();
    }

    public class ArticleEventResponse
    {
        public Guid Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int EventType { get; set; }
    }
}
