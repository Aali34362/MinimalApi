using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.GraphQLIntegration;

public class MenuMutation : ObjectGraphType
{
    public MenuMutation(IMenuRepository menuRepository)
    {
        Field<MenuType>("CreateMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<MenuInputType> { Name = "menu" }
            ))
            .Resolve(context => {
                return menuRepository.AddMenu(context.GetArgument<Menu>("menu"));
            });

        Field<MenuType>("UpdateMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" },
                new QueryArgument<MenuInputType> { Name = "menu" }
            ))
            .Resolve(context => {
                return menuRepository.UpdateMenu(context.GetArgument<Guid>("menuId"), context.GetArgument<Menu>("menu"));
            });

        Field<StringGraphType>("DeleteMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" }
            ))
            .Resolve(context => {
                menuRepository.DeleteMenu(context.GetArgument<Guid>("menuId"));
                return "Deleted";
            });
    }
}
