using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Operations;

// LibraryService class - SRP, OCP
public class LibraryService(IBookRepository repository) : IBookOperations
{
    private readonly IBookRepository _repository = repository;

    public void BorrowBook(int bookId)
    {
        var book = _repository.GetBookById(bookId);
        if (book == null)
        {
            Console.WriteLine("Book not found!");
            return;
        }

        if (book.IsBorrowed)
        {
            Console.WriteLine($"The book \"{book.Title}\" is already borrowed.");
            return;
        }

        book.IsBorrowed = true;
        Console.WriteLine($"You have borrowed \"{book.Title}\".");
    }

    public void ReturnBook(int bookId)
    {
        var book = _repository.GetBookById(bookId);
        if (book == null)
        {
            Console.WriteLine("Book not found!");
            return;
        }

        if (!book.IsBorrowed)
        {
            Console.WriteLine($"The book \"{book.Title}\" was not borrowed.");
            return;
        }

        book.IsBorrowed = false;
        Console.WriteLine($"You have returned \"{book.Title}\".");
    }
}
