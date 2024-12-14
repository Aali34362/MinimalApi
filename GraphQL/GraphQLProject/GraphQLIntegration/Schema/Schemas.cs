
namespace GraphQLProject.GraphQLIntegration;


public class RootSchema : GraphQL.Types.Schema
{
    public RootSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<RootQuery>();
        Mutation = serviceProvider.GetRequiredService<RootMutation>();
    }
}

public class MenuSchema : GraphQL.Types.Schema
{
    public MenuSchema(MenuQuery menuQuery, MenuMutation menuMutation)
    {
        Query = menuQuery;
        Mutation = menuMutation;
    }
}

public class CategorySchema : GraphQL.Types.Schema
{
    public CategorySchema(CategoryQuery categoryQuery, CategoryMutation categoryMutation)
    {
        Query = categoryQuery;
        Mutation = categoryMutation;
    }
}

public class ReservationSchema : GraphQL.Types.Schema
{
    public ReservationSchema(ReservationQuery reservationQuery, ReservationMutation reservationMutation)
    {
        Query = reservationQuery;
        Mutation = reservationMutation;
    }
}