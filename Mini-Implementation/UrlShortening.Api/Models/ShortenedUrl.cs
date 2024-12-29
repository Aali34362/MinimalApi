namespace URL_Shortener.Models;

public record ShortenedUrl(string ShortCode, string OriginalUrl, DateTime CreatedAt);

