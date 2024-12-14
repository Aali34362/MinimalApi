using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using GraphQLProject.Services;

namespace GraphQLProject.GraphQLIntegration;

#region Menu
public class MenuType : ObjectGraphType<Menu>
{
    public MenuType(IRepository<Category> categoryRepository)
    {
        Field(m => m.Id);
        Field(m => m.Name);
        Field(m => m.Description);
        Field(m => m.Price);
        Field(m => m.ImageUrl);
        Field(m => m.CategoryId);
        //Nested Queries
        Field<CategoryType>("Category")
             .Arguments(
             new QueryArguments(
                 new QueryArgument<GuidGraphType> { Name = "categoryId" }
             ))
             .Resolve(context => {
                 // Automatically use the parent Menu's CategoryId
                 var categoryId = context.Source.CategoryId;
                 return categoryRepository.GetById(categoryId);
             });
    }
}
public class MenuInputType : InputObjectGraphType
{
    public MenuInputType()
    {
        //Field<GuidGraphType>("id");
        Field<StringGraphType>("name");
        Field<StringGraphType>("description");
        Field<FloatGraphType>("price");
        Field<StringGraphType>("imageurl");
        Field<GuidGraphType>("categoryid");        
    }
}
#endregion

#region Category
public class CategoryType : ObjectGraphType<Category>
{
    public CategoryType(IMenuRepository menuRepository)
    {
        Field(m => m.Id);
        Field(m => m.Name);
        Field(m => m.ImageUrl);
        //Nested Queries
        Field<ListGraphType<MenuType>>("Menus")
        .Resolve(context => {
                return menuRepository.GetAllMenu();
            });
    }
}
public class CategoryInputType : InputObjectGraphType
{
    public CategoryInputType()
    {
        //Field<GuidGraphType>("id");
        Field<StringGraphType>("name");
        Field<StringGraphType>("imageurl");
    }
}
#endregion

#region Reservation
public class ReservationType : ObjectGraphType<Reservation>
{
    public ReservationType()
    {
        Field(m => m.Id);
        Field(m => m.CustomerName);
        Field(m => m.Email);
        Field(m => m.PhoneNumber);
        Field(m => m.PartySize);
        Field(m => m.SpecialRequest);
        Field(m => m.ReservationDate);
    }
}
public class ReservationInputType : InputObjectGraphType
{
    public ReservationInputType()
    {
        //Field<GuidGraphType>("id");
        Field<StringGraphType>("customername");
        Field<StringGraphType>("email");
        Field<StringGraphType>("phonenumber");
        Field<StringGraphType>("specialrequest");
        Field<IntGraphType>("partysize");
        Field<DateTimeGraphType>("reservationdate");
    }
}
#endregion