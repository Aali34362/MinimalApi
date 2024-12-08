using GraphQL.Types;

namespace GraphQLProject.GraphQLIntegration;

public class MenuInputType : InputObjectGraphType
{
    public MenuInputType()
    {
        //Field<GuidGraphType>("id");
        Field<StringGraphType>("name");
        Field<StringGraphType>("description");
        Field<FloatGraphType>("price");
    }
}
