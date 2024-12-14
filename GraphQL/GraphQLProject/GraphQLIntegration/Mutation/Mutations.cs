using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.GraphQLIntegration;

public class RootMutation : ObjectGraphType
{
    public RootMutation()
    {
        Field<MenuMutation>("menuMutation").Resolve(context => new { });
        Field<CategoryMutation>("categoryMutation").Resolve(context => new { });
        Field<ReservationMutation>("reservationMutation").Resolve(context => new { });
    }
}

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


        Field<MenuType>("CreateDBMenu")
           .Arguments(
           new QueryArguments(
               new QueryArgument<MenuInputType> { Name = "menu" }
           ))
           .ResolveAsync(async context => {
               return await menuRepository.AddDBMenu(context.GetArgument<Menu>("menu"));
           });

        Field<MenuType>("UpdateDBMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" },
                new QueryArgument<MenuInputType> { Name = "menu" }
            ))
            .ResolveAsync(async context => {
                return await menuRepository.UpdateDBMenu(context.GetArgument<Guid>("menuId"), context.GetArgument<Menu>("menu"));
            });

        Field<StringGraphType>("DeleteDBMenu")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "menuId" }
            ))
            .ResolveAsync(async context => {
                await menuRepository.DeleteDBMenu(context.GetArgument<Guid>("menuId"));
                return "Deleted";
            });
    }
}

public class CategoryMutation : ObjectGraphType
{
    public CategoryMutation(IRepository<Category> categoryRepository)
    {
        Field<CategoryType>("CreateCategory")
           .Arguments(
           new QueryArguments(
               new QueryArgument<CategoryInputType> { Name = "category" }
           ))
           .ResolveAsync(async context => {
               return await categoryRepository.Add(context.GetArgument<Category>("category"));
           });

        Field<CategoryType>("UpdateCategory")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "categoryId" },
                new QueryArgument<CategoryInputType> { Name = "category" }
            ))
            .ResolveAsync(async context => {
                return await categoryRepository.Update(context.GetArgument<Guid>("categoryId"), context.GetArgument<Category>("category"));
            });

        Field<StringGraphType>("DeleteCategory")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "categoryId" }
            ))
            .ResolveAsync(async context => {
                await categoryRepository.Delete(context.GetArgument<Guid>("categoryId"));
                return "Deleted";
            });
    }
}

public class ReservationMutation : ObjectGraphType
{
    public ReservationMutation(IRepository<Reservation> reservationRepository)
    {
        Field<ReservationType>("CreateReservation")
           .Arguments(
           new QueryArguments(
               new QueryArgument<ReservationInputType> { Name = "reservation" }
           ))
           .ResolveAsync(async context => {
               return await reservationRepository.Add(context.GetArgument<Reservation>("reservation"));
           });

        Field<ReservationType>("UpdateReservation")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "reservationId" },
                new QueryArgument<ReservationInputType> { Name = "reservation" }
            ))
            .ResolveAsync(async context => {
                return await reservationRepository.Update(context.GetArgument<Guid>("reservationId"), context.GetArgument<Reservation>("reservation"));
            });

        Field<StringGraphType>("DeleteReservation")
            .Arguments(
            new QueryArguments(
                new QueryArgument<GuidGraphType> { Name = "reservationId" }
            ))
            .ResolveAsync(async context => {
                await reservationRepository.Delete(context.GetArgument<Guid>("reservationId"));
                return "Deleted";
            });
    }
}
