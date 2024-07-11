using System.Collections.Generic;

namespace OopsRelatedConceptImplementation.OopsImplementation.LSP;

/*public static class LiskovTest<TStack1, TStack2, TStack3> where TStack1 : IStack<T>, new() where TStack2 : IStack<int>, new() where TStack3 : IStack<int>, new()
{
    private static void TryOut(string testName, TStack1<int> stack, Action<IStack<int>> action, Func<IStack<int>, bool> assertion)
    {
        try
        {
            Console.WriteLine($"Test: {testName}");

            // Perform action on the stack
            action(stack);

            // Check assertion
            bool result = assertion(stack);
            Console.WriteLine($"Result: {(result ? "Passed" : "Failed")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test failed with exception: {ex.Message}");
        }
        Console.WriteLine();
    }
}*/
