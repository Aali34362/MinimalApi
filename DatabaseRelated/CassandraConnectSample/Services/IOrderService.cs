using CassandraConnectSample.UnitOfWork;

namespace CassandraConnectSample.Services;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(Guid id);
}

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateOrderAsync(Order order)
    {
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _unitOfWork.Orders.GetByIdAsync(id);
    }
}