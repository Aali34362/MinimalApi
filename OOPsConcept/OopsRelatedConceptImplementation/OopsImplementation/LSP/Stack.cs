namespace OopsRelatedConceptImplementation.OopsImplementation.LSP;

class Stack<T>
{
    public virtual int Count { get; } = 0;
    public virtual void Push(T item) { }
    public virtual T Pop() => default!;
}

class UniqueStack<T> : Stack<T>
{
    public override int Count { get; } = 0;
    public override void Push(T item)
    {
       
    }
    public override T Pop() => default!;

}