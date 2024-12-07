using GraphQLProject.Interfaces;
using GraphQLProject.Services;

namespace GraphQLProject.ConfigureServices;

public static class DependencyServices
{
    public static void AddDependencyServices(this IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddTransient<IMenuRepository, MenuRepository>();
    }
}
