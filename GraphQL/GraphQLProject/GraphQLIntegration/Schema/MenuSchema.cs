
namespace GraphQLProject.GraphQLIntegration;

public class MenuSchema : GraphQL.Types.Schema
{
    public MenuSchema(MenuQuery menuQuery, MenuMutation menuMutation)
    {
        Query = menuQuery;
        Mutation = menuMutation;
    }
}
