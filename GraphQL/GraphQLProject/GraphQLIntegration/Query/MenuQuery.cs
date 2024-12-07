using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;

namespace GraphQLProject.GraphQLIntegration;

public class MenuQuery : ObjectGraphType
{
    public MenuQuery(IMenuRepository menuRepository)
    {
        Field<ListGraphType<MenuType>>("Menus")
            .Resolve(context => {
                return menuRepository.GetAllMenu();
            });

        Field<MenuType>("Menu")
            .Arguments( 
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" } 
            ))
            .Resolve(context => {
                return menuRepository.GetMenuById(context.GetArgument<Guid>("menuId"));
            });
    }
}
