namespace GraphQLProject.Models;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
    public double Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty!;
    public Guid CategoryId { get; set; }
}
