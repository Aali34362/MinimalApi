using Dumpify;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services;

public class CustomersService(ILogger<CustomersService> logger) : Customer.CustomerBase
{
    private readonly ILogger<CustomersService> _logger = logger;

    public override Task<CustomerModel> GetCustomerInfo(CustomerLookUpModel request, ServerCallContext context)
    {
        _logger.Dump();
        request.Dump();
        return Task.FromResult(request.UserId switch
        {
            1 => new CustomerModel{ FirstName = "Jane", LastName = "Doe" },
            2 => new CustomerModel{ FirstName = "Jamie", LastName = "Smith" },
            _ => new CustomerModel { FirstName = "Greg", LastName = "Thomas" },
        });
    }
}
