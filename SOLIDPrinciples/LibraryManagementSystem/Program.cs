
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Operations;
using LibraryManagementSystem.Repositories;

IBookRepository repository = new BookRepository();
IBookOperations libraryService = new LibraryService(repository);

while (true)
{
    Console.WriteLine("\nLibrary Management System");
    Console.WriteLine("1. View all books");
    Console.WriteLine("2. Borrow a book");
    Console.WriteLine("3. Return a book");
    Console.WriteLine("4. Exit");
    Console.Write("Select an option: ");

    string option = Console.ReadLine()!;
    switch (option)
    {
        case "1":
            DisplayBooks(repository.GetAllBooks());
            break;
        case "2":
            Console.Write("Enter the book ID to borrow: ");
            if (int.TryParse(Console.ReadLine(), out int borrowId))
            {
                libraryService.BorrowBook(borrowId);
            }
            else
            {
                Console.WriteLine("Invalid input!");
            }
            break;
        case "3":
            Console.Write("Enter the book ID to return: ");
            if (int.TryParse(Console.ReadLine(), out int returnId))
            {
                libraryService.ReturnBook(returnId);
            }
            else
            {
                Console.WriteLine("Invalid input!");
            }
            break;
        case "4":
            Console.WriteLine("Exiting the system. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}


static void DisplayBooks(IEnumerable<Book> books)
{
    Console.WriteLine("\nBooks in Library:");
    foreach (var book in books)
    {
        Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Borrowed: {book.IsBorrowed}");
    }
}
/*
SRP:
Book manages only book properties.
BookRepository handles book storage.
LibraryService handles borrowing and returning logic.

OCP:
You can extend LibraryService (e.g., adding new features) without modifying its existing code.

LSP:
LibraryService depends on the IBookOperations interface, and any implementation of this interface can replace it.

ISP:
Two interfaces: IBookOperations (borrowing/returning) and IBookRepository (repository logic). This separation avoids forcing classes to implement unnecessary methods.

DIP:
LibraryService depends on the IBookRepository abstraction rather than a concrete implementation.
 */