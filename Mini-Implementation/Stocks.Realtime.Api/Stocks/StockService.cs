using Dapper;
using Npgsql;
using Stocks.Realtime.Api.RealTime;

namespace Stocks.Realtime.Api.Stocks;

internal sealed class StockService(
    ActiveTickerManager activeTickerManager,
    NpgsqlDataSource dataSource,
    StocksClient stocksClient,
    ILogger<StockService> logger)
{
    public async Task<StockPriceResponse?> GetLatestStockPrice(string ticker)
    {
        try
        {
            // First, try to get the latest price from the database
            StockPriceResponse? dbPrice = await GetLatestPriceFromDatabase(ticker);
            if (dbPrice is not null)
            {
                activeTickerManager.AddTicker(ticker);
                return dbPrice;
            }

            // If not found in the database, fetch from the external API
            StockPriceResponse? apiPrice = await stocksClient.GetDataForTicker(ticker);

            if (apiPrice == null)
            {
                logger.LogWarning("No data returned from external API for ticker: {Ticker}", ticker);
                return null;
            }

            // Save the new price to the database
            await SavePriceToDatabase(apiPrice);

            activeTickerManager.AddTicker(ticker);

            return apiPrice;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching stock price for ticker: {Ticker}", ticker);
            throw;
        }
    }

    private async Task<StockPriceResponse?> GetLatestPriceFromDatabase(string ticker)
    {
        const string sql =
            """
            SELECT ticker, price, timestamp
            FROM public.stock_prices
            WHERE ticker = @Ticker
            ORDER BY timestamp DESC
            LIMIT 1
            """;

        using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        StockPriceRecord? result = await connection.QueryFirstOrDefaultAsync<StockPriceRecord>(sql, new
        {
            Ticker = ticker
        });

        if (result is not null)
        {
            return new StockPriceResponse(result.Ticker, result.Price);
        }

        return null;
    }

    private async Task SavePriceToDatabase(StockPriceResponse price)
    {
        const string sql =
            """
            INSERT INTO public.stock_prices (ticker, price, timestamp)
            VALUES (@Ticker, @Price, @Timestamp)
            """;

        using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync(sql, new
        {
            price.Ticker,
            price.Price,
            Timestamp = DateTime.UtcNow
        });
    }

    private sealed record StockPriceRecord(string Ticker, decimal Price, DateTime Timestamp);
}
