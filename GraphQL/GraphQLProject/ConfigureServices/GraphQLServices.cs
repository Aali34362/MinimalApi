using GraphQL;
using GraphQL.Types;
using GraphQLProject.Models;

namespace GraphQLProject.ConfigureServices;

public static class GraphQLServices
{
    public static void AddGraphQLServices(this IServiceCollection serviceDescriptors)
    {
        
        serviceDescriptors
            .AddGraphQL(b => 
                b.AddAutoSchema<ISchema>()
                .AddSystemTextJson());

        serviceDescriptors
            .AddOptions<JwtOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
