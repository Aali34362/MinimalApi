// See https://aka.ms/new-console-template for more information
using OopsRelatedConceptImplementation.OopsImplementation.AbstractConcept;

Console.WriteLine("Hello, World!");

Base b = new Base();
Base bd = new Derived();
Derived d = new Derived();

Base bp = new Proxy();
Proxy p = new Proxy();

IAmTheSavior x = new Bases();
IAmTheSavior y = new Proxys();
IAmTheSavior z = new Maybeproxy();
IAmTheSavior u = new TrueProxy(z);

Console.WriteLine("Classes : without virtual keyword");
b.Method();
bd.Method();
d.Method();
Console.WriteLine(new string('-',40));
Console.WriteLine("Classes : virtual keyword");
b.Method1();
bd.Method1();
d.Method1();
Console.WriteLine(new string('-', 40));
Console.WriteLine("Classes : virtual keyword");
bp.Method1();
p.Method1();
Console.WriteLine("Classes : without virtual keyword");
bp.Method();
p.Method();
Console.WriteLine(new string('-', 40));
Console.WriteLine("Interfaces : without virtual keyword and Data Hiding using new keyword");
x.Methods();    // x -> IAmTheSavior in Base    -> Base      -> Base.Method()
y.Methods();    // y -> IAmTheSavior in Proxy    -> Base      -> Base.Method()
z.Methods();    // z -> IAmTheSavior in M/Proxy    -> M/Proxy      -> M/Proxy.Method()
u.Methods();


///////////////////////////////////////////////////////
Console.WriteLine(new string('-', 40));
Console.WriteLine("Vechile and Car (Abstract Class)");

Car myCar = new Car("Toyota", "Camry");

// Output the initial state of the car
Console.WriteLine($"Car Make: {myCar.Make}");
Console.WriteLine($"Car Model: {myCar.Model}");
Console.WriteLine($"Engine Status: {myCar.EngineStatus}");
Console.WriteLine($"Screen Context: {myCar.ScreenContext}");

// Starting the engine of the car
myCar.StartEngine();

// Output the updated state of the car after starting the engine
Console.WriteLine($"Engine Status after starting: {myCar.EngineStatus}");
Console.WriteLine($"Screen Context after starting: {myCar.ScreenContext}");


//////////////////////////////////////////////////////////
Console.WriteLine(new string('-', 40));
Console.WriteLine("Animal and Cat (Abstract Class)");
Animal animal = new Animal();
Animal animalcat = new Cat();
Cat cat = new Cat();

Console.WriteLine("VirtA");
animal.VirtA();
animalcat.VirtA();
cat.VirtA();

Console.WriteLine("VirtB");
animal.VirtB();
animalcat.VirtB();
cat.VirtB();

Console.WriteLine("Testbed Calls");
Testbed.Call(nameof(Animal.VirtA), x => x.VirtA(), animal, animalcat);
Testbed.Call(nameof(Animal.VirtA), x => x.VirtA(), cat);

////////////////////////////////////////////////////////
Console.WriteLine(new string('-', 40));
Console.WriteLine("Private Set; and Init;");
// Create a Person instance
Person person = new Person("John", 30);
// Accessing properties from within the class
Console.WriteLine($"Name: {person.Name}");
Console.WriteLine($"Age: {person.Age}");
// Attempting to modify the properties from outside the class
// This will result in a compile-time error
// person.Name = "Jane"; // Error: 'Person.Name' is read-only
// person.Age = 35;      // Error: 'Person.Age' is read-only
////////////////////////////////////////////////////////