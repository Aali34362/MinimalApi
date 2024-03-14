namespace OopsRelatedConceptImplementation.OopsImplementation;

public class Person
{
    public string Name { get; private set; } // Read-write property accessible within the class
    public int Age { get; init; } // Init-only property accessible within the class

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
