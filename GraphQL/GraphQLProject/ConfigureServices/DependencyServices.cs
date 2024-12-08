using GraphQL.Types;
using GraphQLProject.GraphQLIntegration;
using GraphQLProject.Interfaces;
using GraphQLProject.Services;

namespace GraphQLProject.ConfigureServices;

public static class DependencyServices
{
    public static void AddDependencyServices(this IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddTransient<IMenuRepository, MenuRepository>();
        serviceDescriptors.AddTransient<MenuType>();
        serviceDescriptors.AddTransient<MenuInputType>();
        serviceDescriptors.AddTransient<MenuQuery>();
        serviceDescriptors.AddTransient<MenuMutation>();
        serviceDescriptors.AddTransient<ISchema, MenuSchema>();
    }
}
