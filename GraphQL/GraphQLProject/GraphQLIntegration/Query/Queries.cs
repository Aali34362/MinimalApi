using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.GraphQLIntegration;

public class RootQuery : ObjectGraphType
{
    public RootQuery()
    {
        Field<MenuQuery>("menuQuery").Resolve(context => new { });
        Field<CategoryQuery>("categoryQuery").Resolve(context => new { });
        Field<ReservationQuery>("reservationQuery").Resolve(context => new { });
    }
}

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

        Field<ListGraphType<MenuType>>("DBMenus")
            .Resolve(context => {
                return menuRepository.GetAllDBMenu();
            });

        Field<MenuType>("DBMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" }
            ))
            .Resolve(context => {
                return menuRepository.GetDBMenuById(context.GetArgument<Guid>("menuId"));
            });
    }
}

public class CategoryQuery : ObjectGraphType
{
    public CategoryQuery(IRepository<Category> categoryRepository)
    {
        Field<ListGraphType<CategoryType>>("Categories")
            .Resolve(context => {
                return categoryRepository.GetList();
            });

        Field<CategoryType>("Category")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "categoryId" }
            ))
            .Resolve(context => {
                return categoryRepository.GetById(context.GetArgument<Guid>("categoryId"));
            });
    }
}

public class ReservationQuery : ObjectGraphType
{
    public ReservationQuery(IRepository<Reservation> reservationRepository)
    {
        Field<ListGraphType<ReservationType>>("Reservations")
            .Resolve(context => {
                return reservationRepository.GetList();
            });

        Field<ReservationType>("Reservation")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "reservationId" }
            ))
            .Resolve(context => {
                return reservationRepository.GetById(context.GetArgument<Guid>("reservationId"));
            });
    }
}
