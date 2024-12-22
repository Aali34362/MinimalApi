namespace LibraryManagementSystem.Operations;

// Interface segregation - ISP
public interface IBookOperations
{
    void BorrowBook(int bookId);
    void ReturnBook(int bookId);
}
