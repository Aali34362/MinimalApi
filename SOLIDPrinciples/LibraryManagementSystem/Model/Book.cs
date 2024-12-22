namespace LibraryManagementSystem.Model;

// Book class - SRP
public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsBorrowed { get; set; }
}
