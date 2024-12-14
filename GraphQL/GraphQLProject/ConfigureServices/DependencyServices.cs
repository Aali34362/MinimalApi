using GraphQL.Types;
using GraphQLProject.GraphQLIntegration;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using GraphQLProject.Services;

namespace GraphQLProject.ConfigureServices;

public static class DependencyServices
{
    public static void AddDependencyServices(this IServiceCollection serviceDescriptors)
    {
        // Register repositories
        serviceDescriptors.AddScoped<IRepository<Category>, CategoryRepository>();
        serviceDescriptors.AddScoped<IRepository<Reservation>, ReservationRepository>();

        // Register other dependencies
        serviceDescriptors.AddTransient<IMenuRepository, MenuRepository>();        
        serviceDescriptors.AddTransient<MenuType>();
        serviceDescriptors.AddTransient<MenuInputType>();
        serviceDescriptors.AddTransient<MenuQuery>();
        serviceDescriptors.AddTransient<MenuMutation>();
        serviceDescriptors.AddTransient<ISchema, MenuSchema>();
    }
}
