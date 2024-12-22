using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Repositories;

// BookRepository class - SRP, DIP
public class BookRepository : IBookRepository
{
    private readonly List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", IsBorrowed = false },
            new Book { Id = 2, Title = "Design Patterns", IsBorrowed = false },
            new Book { Id = 3, Title = "Refactoring", IsBorrowed = false }
        };

    public IEnumerable<Book> GetAllBooks() => books;

    public Book GetBookById(int bookId) => books.Find(book => book.Id == bookId)!;
}
