namespace GraphQLProject.Models;

public class Menu
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
    public double Price { get; set; }
}
