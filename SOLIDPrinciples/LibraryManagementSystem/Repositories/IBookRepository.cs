using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Repositories;

// Interface segregation - ISP
public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    Book GetBookById(int bookId);
}
