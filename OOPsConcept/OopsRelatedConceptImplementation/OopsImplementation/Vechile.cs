namespace OopsRelatedConceptImplementation.OopsImplementation;

public class Vechile
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string EngineStatus { get; private set; } ="Off";
    public Vechile(string make, string model)
    {
        Make = make;
        Model = model;
    }
}

public class Car : Vechile
{
    public Car(string make, string model) : base(make, model)
    {
    }
}

/*
 * private set;: 
 * This means that the property can only be set from within the same class. 
 * It allows the property to be modified only by other methods or 
 * constructors within the class itself. 
 * Outside the class, the property appears read-only.
 * 
 * init;: 
 * This is a new feature introduced in C# 9.0. 
 * It allows the property to be set only during initialization,
 * typically within a constructor or object initializer. 
 * After initialization, the property becomes read-only and cannot be modified.
 * 
 * set;: 
 * This is the standard way to define a property setter in C#. 
 * It allows the property to be set from anywhere within the same assembly or 
 * from derived classes if the property is not explicitly marked as private.
 */