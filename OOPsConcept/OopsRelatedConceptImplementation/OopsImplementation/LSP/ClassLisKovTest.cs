//using static OopsRelatedConceptImplementation.OopsImplementation.LSP.LiskovTest<Stack<int> Stack<int>, UniqueStack<int>>;
using System.Collections;
namespace OopsRelatedConceptImplementation.OopsImplementation.LSP;
static class ClassLisKovTest
{
    public static void Run()
    {
        Console.WriteLine($"Testing classes : ".ToUpper());
        Console.WriteLine();

        //TryOut("Push leaves stack non-empty", stack => stack.Push(1), stack => stack.Count > 0);
        //TryOut("Multiple pushes leave stack non-empty", stack => stack.Push(1,2), stack => stack.Count > 0);
        //TryOut("Multiple pushes leave stack non-empty", stack => stack.Push(1,1), stack => stack.Count > 0);

        //TryOut("Pop succeeds on non-empty stack", stack => stack.Push(1), stack => stack.Pop() is int _);
        //TryOut("Pop succeeds on non-empty stack", stack => stack.Push(1,2), stack => stack.Pop() is int _);
        //TryOut("Pop succeeds on non-empty stack", stack => stack.Push(1,1), stack => stack.Pop() is int _);

        //TryOut("Stack works as a LIFO structure", stack => stack.Push(1,2), stack => stack.Pop() >= stack.Pop());
        //TryOut("Stack works as a LIFO structure", stack => stack.Push(1,2,3), stack => {stack.Pop();}, stack >= stack.Pop() >= stack.Pop());

        //TryOut("Item is present after push", stack => stack.Push(1,2), stack => stack.Contains(1));
        //TryOut("Item is present after push", stack => stack.Push(1,1), stack => stack.Contains(1));

        //TryOut("Push increments Count", stack => stack.Push(1), stack => stack.Push(2), (before,after) => after.Count == before.Count + 1);
        //TryOut("Push increments Count", stack => stack.Push(1), stack => stack.Push(1), (before,after) => after.Count == before.Count + 1);

        //TryOut("Pop decrements Count", stack => stack.Push(1,2), stack => {stack.Pop();}, (before,after) => after.Count == before.Count - 1);
        //TryOut("Pop decrements Count", stack => stack.Push(1,1), stack => {stack.Pop();}, (before,after) => after.Count == before.Count - 1);


        Stack<int> a = new Stack<int>();
        Stack<int> b = new UniqueStack<int>();

        a.Push(1);
        a.Pop();

        b.Push(1);
        b.Pop();

        Console.WriteLine();
        Console.WriteLine(new string('=',82));
        Console.WriteLine();
    }
}

file class Stack<T> : IEnumerable<T>
{
    private List<T> Items { get; } = new();
    public virtual int Count => Items.Count;
    public virtual void Push(T item, params T[] more) 
    {
        Items.Add(item);
        foreach (var additional in more) Items.Add(additional);
    }
    public virtual T Pop()
    {
        if (Items.Count == 0) throw new InvalidOperationException("Stack is Empty");
        T result = Items[^1];
        Items.RemoveAt(Items.Count);
        return result;
    }
    public IEnumerator<T> GetEnumerator() { for (int i = Items.Count - 1; i >= 0; i--) yield return Items[i]; }
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

file class UniqueStack<T> : Stack<T>
{
    public override int Count { get; } = 0;
    public override void Push(T item, params T[] more) {
        PushSingle(item);
        foreach (var additional in more) PushSingle(additional);
    }

    private void PushSingle(T item)
    {
        if (this.Contains(item)) return;
        base.Push(item);
    }

    public override T Pop() => default!;
}
