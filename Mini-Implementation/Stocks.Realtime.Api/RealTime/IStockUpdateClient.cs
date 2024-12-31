namespace Stocks.Realtime.Api.Realtime;

public interface IStockUpdateClient
{
    Task ReceiveStockPriceUpdate(StockPriceUpdate update);
}
