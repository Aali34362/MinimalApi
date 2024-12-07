using GraphQLProject.GraphQLIntegration.Query;

namespace GraphQLProject.GraphQLIntegration.Schema;

public class MenuSchema : GraphQL.Types.Schema
{
    public MenuSchema(MenuQuery menuQuery)
    {
        Query = menuQuery;
    }
}
