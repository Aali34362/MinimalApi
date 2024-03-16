namespace OopsRelatedConceptImplementation.OopsImplementation.AbstractConcept;



class Animal
{
    public virtual void VirtA() => Console.WriteLine("Base.VirtA");
    public virtual void VirtB() => Console.WriteLine("Base.VirtA");
}

class Cat : Animal
{
    public override void VirtA() => Console.WriteLine("Derived.VirtA");
}
