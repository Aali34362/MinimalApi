using GraphQL;
using GraphQL.Types;

namespace GraphQLProject.ConfigureServices;

public static class GraphQLServices
{
    public static void AddGraphQLServices(this IServiceCollection serviceDescriptors)
    {
        
        serviceDescriptors
            .AddGraphQL(b => 
                b.AddAutoSchema<ISchema>()
                .AddSystemTextJson());

    }
}
