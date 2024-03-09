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

    public override async Task GetNewCustomer(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
    {
        List<CustomerModel> customers = new() {
         new CustomerModel { FirstName = "Tim", LastName ="Corey", Age = 41, EmailAddress = "tim@iamtimcorey.com", IsAlive = false },
         new CustomerModel { FirstName = "ABC", LastName ="XYZ", Age = 31, EmailAddress = "time@iamTimecorey.com", IsAlive = true },
         new CustomerModel { FirstName = "LMN", LastName ="TUV", Age = 15, EmailAddress = "time@iamTimecorey.com", IsAlive = true },
        };

        foreach(var cust in customers)
        {
            await Task.Delay(1000);
            cust.Dump();
            await responseStream.WriteAsync(cust);
        }
    }
}
