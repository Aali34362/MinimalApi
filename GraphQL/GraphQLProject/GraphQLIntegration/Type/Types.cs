using GraphQL.Types;
using GraphQLProject.Models;

namespace GraphQLProject.GraphQLIntegration;

#region Menu
public class MenuType : ObjectGraphType<Menu>
{
    public MenuType()
    {
        Field(m => m.Id);
        Field(m => m.Name);
        Field(m => m.Description);
        Field(m => m.Price);
        Field(m => m.ImageUrl);
        Field(m => m.CategoryId);
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
        Field<FloatGraphType>("imageurl");
        Field<GuidGraphType>("categoryid");
    }
}
#endregion

#region Category
public class CategoryType : ObjectGraphType<Category>
{
    public CategoryType()
    {
        Field(m => m.Id);
        Field(m => m.Name);
        Field(m => m.ImageUrl);
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