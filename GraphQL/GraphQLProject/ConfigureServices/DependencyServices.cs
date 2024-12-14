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
        serviceDescriptors.AddTransient<IMenuRepository, MenuRepository>();        
        serviceDescriptors.AddTransient<MenuType>();
        serviceDescriptors.AddTransient<MenuInputType>();
        serviceDescriptors.AddTransient<MenuQuery>();
        serviceDescriptors.AddTransient<MenuMutation>();
        serviceDescriptors.AddTransient<ISchema, MenuSchema>();

        serviceDescriptors.AddScoped<IRepository<Category>, CategoryRepository>();
        serviceDescriptors.AddTransient<CategoryType>();
        serviceDescriptors.AddTransient<CategoryInputType>();
        serviceDescriptors.AddTransient<CategoryQuery>();
        serviceDescriptors.AddTransient<CategoryMutation>();
        serviceDescriptors.AddTransient<ISchema, CategorySchema>();

        serviceDescriptors.AddScoped<IRepository<Reservation>, ReservationRepository>();
        serviceDescriptors.AddTransient<ReservationType>();
        serviceDescriptors.AddTransient<ReservationInputType>();
        serviceDescriptors.AddTransient<ReservationQuery>();
        serviceDescriptors.AddTransient<ReservationMutation>();
        serviceDescriptors.AddTransient<ISchema, ReservationSchema>();
    }
}
