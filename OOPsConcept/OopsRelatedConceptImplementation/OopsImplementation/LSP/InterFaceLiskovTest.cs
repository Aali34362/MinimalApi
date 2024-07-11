namespace OopsRelatedConceptImplementation.OopsImplementation.LSP;

static class InterFaceLiskovTest
{
    public static void Run()
    {
        Console.WriteLine($"Testing Interface : ".ToUpper());
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine(new string('=', 82));
        Console.WriteLine();
    }
}

file interface  IStackable<T> : IEnumerable<T>
{
    
}
