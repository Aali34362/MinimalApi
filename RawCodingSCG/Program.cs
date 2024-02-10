using RawCodingSCG.FrameWork;
using SampleGenerator;
//using static System.Net.Mime.MediaTypeNames;
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
var car = new Car();
Car.Do();
Console.WriteLine(new String('-', 50));

///////////This Class is generated From Source Code
foreach(string name in ClassNames.Names)
{
    Console.WriteLine(name);
}
Console.WriteLine(ClassNames.MapAllEndpoints("Hello World"));
Console.WriteLine(ClassNames.MapAllEndpoint(1));

////////////////////////////////////IDisposable///////////////////////////
//Using Statement -> this class must implement IDisposable interface will properly call IDisposable interface every time it is called
using DemoResource demo = new();
try { demo.DoWork(); } catch (Exception ex) { Console.WriteLine(ex.Message); }
Console.WriteLine( new String('-',50));

////////////////////////////////////////////////////////////////////////////

Dictionary<Book, string> whatt = new(Book.IdEqualityComparer);
Dictionary<Book, string> good = new();
HashSet<Book> hnh = new();

List<Book> books = new();

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
    public Entity(TId id) => Id = id;
    public static IEqualityComparer<Entity<TId>> IdEqualityComparer =>
        EqualityComparer<Entity<TId>>.Create((x, y) => 
        x is null ? y is null
            : y is not null && x.GetType() == y.GetType() && x.Id.Equals(y.Id));
}
class Book : Entity<BookId>
{
    public BookId Id { get; private set; }
    public string Title { get; set; }

    //public Book(BookId id, string title) => (Id, Title) = (id, title);
    public Book(BookId id, string title) : base(id) => Title = title;

    public static Book CreateNew() => new(new(Guid.NewGuid()), string.Empty);
}
readonly record struct BookId(Guid Value);

//////////////////////////////////////////////////////////////////////////////////



public class Car
{
    [Give("Print")]
    public static void Do()
    {
        Console.WriteLine("Do");
    }
}

/*public partial class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Give("FullNames")]
    public static partial string FullName();
}*/

public static class Functions
{
    [Define]
    public static void Print()
    {
       Console.WriteLine("Hello, World!");
    }

    /*[Define]
    public static string FullNames()
    {
        var type = GetType();
        var firstNameP = type.GetProperty("FirstName");
        var lastNameP = type.GetProperty("LastName");
        var fn = (string)firstNameP.GetValue(this);
        var ln = (string)lastNameP.GetValue(this);
        return $"{fn} {ln}";
    }*/

    public static void Save()
    {
        Console.WriteLine("Hello, Worlds!");
    }
}