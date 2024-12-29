using Dapper;
using Microsoft.Extensions.Caching.Hybrid;
using Npgsql;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using URL_Shortener.Models;

namespace URL_Shortener.Services;

internal sealed class UrlShorteningService(
    NpgsqlDataSource dataSource,
    ILogger<UrlShorteningService> logger,
    HybridCache cache,
    IHttpContextAccessor httpContextAccessor)
{
    private static string GenerateShortCode()
    {
        const int length = 7;
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRESTUVWXYZ0123456789";
        var chars = Enumerable.Range(0, length)
            .Select(_ => alphabet[Random.Shared.Next(alphabet.Length)])
            .ToArray();
        return new string(chars);
    }

    private const int MaxRetries = 3;
    private static readonly Meter Meter = new("UrlShortening.Api");
    private static readonly Counter<int> RedirectsCounter = Meter.CreateCounter<int>(
        "url_shortener.redirects",
        "The number of successful redirects"
        );
    private static readonly Counter<int> FailedRedirectsCounter = Meter.CreateCounter<int>(
        "url_shortener.failed_redirects",
        "The number of failed redirects"
        );

    public async Task<string> ShortenUrl(string originalUrl)
    {
        for (int attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var shortCode = GenerateShortCode();
                const string sql =
                    """
            Insert into shortened_urls (short_code, original_url)
            values (@ShortCode, @OriginalUrl)
            Returning short_code;
            """;
                await using var connection = await dataSource.OpenConnectionAsync();
                var result = await connection.QuerySingleAsync<string>(sql, new { ShortCode = shortCode, OrignialUrl = originalUrl });
                await cache.SetAsync(shortCode, originalUrl);
                return result;
            }
            catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                if (attempt == MaxRetries)
                {
                    logger.LogError(ex,
                        "Failed to generate unique short code after {MaxRetries} attempts",
                        MaxRetries);
                    throw new InvalidOperationException("Failed to generate unique short code.", ex);
                }
                logger.LogWarning(
                    "Short code collision occurred. Retrying... (Attempt {Attempt} of {MaxRetries})",
                    attempt + 1, MaxRetries);
            }
        }
        throw new InvalidOperationException("Failed to generate unique short code.");
    }
    public async Task<string> GetOriginalUrl(string shortCode)
    {
        var originalUrl = await cache.GetOrCreateAsync(shortCode, async token =>
        {
            const string sql =
            """
            Select original_url
            From shortened_urls
            Where short_code = @ShortCode;
            """;
            await using var connection = await dataSource.OpenConnectionAsync();
            var originalUrl = await connection.QuerySingleOrDefaultAsync<string>(sql, new { ShortCode = shortCode });
            return originalUrl!;
        });
        if (originalUrl is null)
            FailedRedirectsCounter.Add(1, new TagList { { "short_code", shortCode } });
        else
        {
            await RecordVisit(shortCode);
            RedirectsCounter.Add(1, new TagList { { "short_code", shortCode } });
        }
        return originalUrl;
    }
    private async Task RecordVisit(string shortCode)
    {
        var context = httpContextAccessor.HttpContext;
        var userAgent = context?.Request.Headers.UserAgent.ToString();
        var referer = context?.Request.Headers.Referer.ToString();

        const string sql =
            """
            Insert into url_visits(short_code, user_agent, referer)
            values (@ShortCode, @UserAgent, @Referer);
            """;
        await using var connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync(sql, new
        {
            ShortCode = shortCode,
            UserAgent = userAgent,
            Referer = referer
        });
    }
    public async Task<IEnumerable<ShortenedUrl>> GetAllUrls()
    {
        const string sql =
            """
            Select short_code as ShortCode, original_url as OriginalUrl, created_at as CreatedAt
            From shortened_urls
            Order by created_at Desc;
            """;
        await using var connection = await dataSource.OpenConnectionAsync();
        return await connection.QueryAsync<ShortenedUrl>(sql);
    }
}
