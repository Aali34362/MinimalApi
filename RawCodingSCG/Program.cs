using RawCodingSCG.FrameWork;
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

Console.WriteLine("Hello, World");

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