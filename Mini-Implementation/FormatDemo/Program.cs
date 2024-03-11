// See https://aka.ms/new-console-template for more information
using FormatDemo.Model;
using FormatDemo.PropertyInjector;
using FormatDemo.TypesofLifeTimeServices;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

PersonModel person = new() { FirstName = "ABC", LastName = "XYZ", MiddleName = "LMN", Title = "MR" };
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

///////////////////////////////////

var serviceProvider = new ServiceCollection()
            .AddTransient<IUniqueIdGeneratorTransient, TransientServices>()  // Register MyService as a transient service
            .AddScoped<IUniqueIdGeneratorScoped, ScopedServices>()
            .AddSingleton<IUniqueIdGeneratorSingleton, SingletonServices>()
            .AddScoped<IOperationScoped, Opertion>()
            .AddSingleton<IOperationSingleton, Opertion>()
            .AddScoped<IOperationSingletonInstance>(a => new Opertion(Guid.Empty))
            .AddTransient<DependencyService>()
            .AddTransient<DependencyService1>()
            .BuildServiceProvider();

// Resolve the service
//var myService = serviceProvider.GetRequiredService<ITransientServices>();

// Use the service
for (int i = 0; i < 20; i++)
{
    // Resolve the service
    var TransientService = serviceProvider.GetRequiredService<IUniqueIdGeneratorTransient>();
    string transientGuid1 = TransientService.GenerateUniqueId();
    string transientGuid2 = TransientService.GenerateUniqueId();
    Console.WriteLine($"Transient Service 1 : {transientGuid1} count : {i}");
    Console.WriteLine($"Transient Service 1 : {transientGuid2} count : {i}");
    Console.WriteLine();
    var ScopedService = serviceProvider.GetRequiredService<IUniqueIdGeneratorScoped>();
    string scopedGuid1 = ScopedService.GenerateUniqueId();
    string scopedGuid2 = ScopedService.GenerateUniqueId();
    Console.WriteLine($"Scoped Service 1 : {scopedGuid1} count : {i}");
    Console.WriteLine($"Scoped Service 1 : {scopedGuid2} count : {i}");
    Console.WriteLine();

}
var SingletonService = serviceProvider.GetRequiredService<IUniqueIdGeneratorSingleton>();
Console.WriteLine($"Singleton Service : {SingletonService.GenerateUniqueId()}");
////////////////////////////////////
