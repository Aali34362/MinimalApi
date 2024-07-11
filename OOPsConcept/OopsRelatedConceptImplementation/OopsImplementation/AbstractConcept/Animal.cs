namespace OopsRelatedConceptImplementation.OopsImplementation.AbstractConcept;

interface IamPolymorphicToo
{
    void VirtA();
    void NonVirtC();
}

class Animal
{
    public virtual void VirtA() => Console.WriteLine("Animal.VirtA");
    public virtual void VirtB() => Console.WriteLine("Animal.VirtB");
    public void NonVirtC() => Console.WriteLine("Animl.NonVirtC");
}

class Cat : Animal
{
    public override void VirtA() => Console.WriteLine("Cat.VirtA");
}
