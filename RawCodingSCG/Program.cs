using RawCodingSCG.FrameWork;
using static System.Net.Mime.MediaTypeNames;
////////////////////////////Source Generator////////////////////////////////
/*
 * Links
 * https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
 * https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md
 * https://roslynquoter.azurewebsites.net/
 * 
 * https://www.youtube.com/watch?v=IUMZH5Z4r00&t=64s
 * https://www.youtube.com/watch?v=zf5j-W11-qo
 * 
 * Defination
 * 
 * Understanding
 * 
 * Usage
 * 
 */

//Test.P();
Console.WriteLine(new String('-', 50));

////////////////////////////////////IDisposable///////////////////////////
//Using Statement -> this class must implement IDisposable interface will properly call IDisposable interface every time it is called
using DemoResource demo = new();
try { demo.DoWork(); } catch (Exception ex) { Console.WriteLine(ex.Message); }
Console.WriteLine( new String('-',50));

////////////////////////////////////////////////////////////////////////////

Dictionary<Book, string> whatt = new();
HashSet<Book> hnh = new();
abstract class Entities<TId> : IEquatable<Entities<TId>> where TId : IEquatable<TId>
{
    public TId Id { get; private set; }
    protected Entities(TId id) => Id = id;

    public bool Equals(Entities<TId>? other)
        => other is not null && GetType() == other.GetType() && Id.Equals(other.Id);
    public override bool Equals(object? obj) 
        => obj is Entities<TId> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    public static bool operator ==(Entities<TId>? left, Entity<TId>? right) =>
        left is null ? right is null : left.Equals(right);
    public static bool operator != (Entities<TId>? left, Entity<TId>? right) =>
        !(left == right);

    
}

abstract class Entity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; private set; }
    protected Entity(TId id) => Id = id;
}
class Book : Entity<BookId>
{
    public BookId Id { get; private set; }
    public string Title { get; set; }

    public Book(BookId id, string title) => (Id, Title) = (id, title);

    public static Book CreateNew() => new(new(Guid.NewGuid()), string.Empty);


}
readonly record struct BookId(Guid Value);

//////////////////////////////////////////////////////////////////////////////////



public partial class Car
{
    [Give("Print")]
    static partial void Do();
}


public static class Functions
{
    [Define]
    public static void Print()
    {
        Console.WriteLine("Hello, World!");
    }

    public static void Save()
    {
        Console.WriteLine("Hello, Worlds!");
    }
}