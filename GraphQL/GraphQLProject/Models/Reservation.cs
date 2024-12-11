namespace GraphQLProject.Models;

public class Reservation : BaseEntity
{
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int PartySize { get; set; }
    public string? SpecialRequest { get; set; }
    public DateTime ReservationDate { get; set; }
}
