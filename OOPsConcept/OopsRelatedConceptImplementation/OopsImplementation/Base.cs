namespace OopsRelatedConceptImplementation.OopsImplementation;

interface IAmTheSavior
{
    void Methods();
}

public class Bases : IAmTheSavior
{
    public void Methods() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Bases Method");

    public virtual void Methods1() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Bases Method1");
}

public class Derives : Bases
{
    public override void Methods1() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Derives Method1");
}

class Proxys : Bases
{
    public new void Methods() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Proxys Method");
}

class Maybeproxy : Bases, IAmTheSavior
{
    public new void Methods() =>
       Console.WriteLine($"obj of {GetType().Name,-12} runs Maybeproxy Method");
}

class TrueProxy : IAmTheSavior
{
    private readonly IAmTheSavior _impl;
    public TrueProxy(IAmTheSavior impl) => _impl = impl;
    public void Methods()
    {
        _impl.Methods();
    }
}

/// <summary>
/// 
/// </summary>
public class Base
{
    public  void Method() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Base Method");

    public virtual void Method1() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Base Method1");
}

public class Derived : Base
{
    public override void Method1() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Derived Method1");
}

class Proxy : Base
{
    public override void Method1() =>
       Console.WriteLine($"obj of {GetType().Name,-12} runs Proxy Method1");
    public new void Method() =>
        Console.WriteLine($"obj of {GetType().Name,-12} runs Proxy Method");
}