// See https://aka.ms/new-console-template for more information
using FormatDemo.Model;
using FormatDemo.PropertyInjector;

Console.WriteLine("Hello, World!");

PersonModel person = new(){ FirstName = "ABC", LastName = "XYZ", MiddleName = "LMN", Title = "MR" };
Console.WriteLine(person);
Console.WriteLine(person.ToString());
Console.WriteLine(person.ToString("FLL"));
Console.WriteLine(person.ToString("LL, T FF M."));
Console.WriteLine(person.ToString("T FF MM LL"));

EmployeeModel employee = new() { FirstName = "ABC", LastName = "XYZ", MiddleName = "LMN" };
Console.WriteLine(employee);
Console.WriteLine(employee.ToString());
Console.WriteLine(employee.ToString("L"));
Console.WriteLine(employee.ToString("Login"));
Console.WriteLine(employee.ToString("S"));
Console.WriteLine(employee.ToString("SORTABLE"));

///////////////////////////////
///// Create an instance of the service
var services = new Service();
// Create an instance of MyClass
var myClass = new MyClass();
// Inject the service into the property
myClass.service = services;
// Call the method that uses the injected service
myClass.PerformAction();
