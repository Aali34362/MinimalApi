using DbUp;
using xyz.math.cal;
string connectionString =
    args.FirstOrDefault() ??
    "Server=UCHIHA_MADARA\\SQLEXPRESS;Database=SocietyManagementSystem;User=UCHIHA_MADARA\\aa882;Password=; MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=true;Connection Timeout=6000;Trusted_Connection=True;";
    EnsureDatabase.For.SqlDatabase(connectionString);
var upgrader = DeployChanges
    .To
    .SqlDatabase(connectionString)
    .LogToConsole()
    .WithScriptsFromFileSystem("Scripts").Build();
var result = upgrader.PerformUpgrade();
if (result.Successful)
{
    Console.WriteLine("Succeess ! ya");
}
else
{
    Console.WriteLine("Failure");
}

//Created from my own Package Hurray
Calculator calculator = new();
Console.WriteLine(calculator.Add(1, 1));
