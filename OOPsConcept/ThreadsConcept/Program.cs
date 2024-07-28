// See https://aka.ms/new-console-template for more information
using ThreadsConcept.BasicMultiThreadCalculator;
using ThreadsConcept.FileReaderWithThread;
using ThreadsConcept.MultithreadedWebScraper;
using ThreadsConcept.ThreadedChatServer;


////MultithreadedCalculator
////MultithreadedCalculator calculator = new MultithreadedCalculator();
////Console.WriteLine("Multithreaded Calculator");
////Console.WriteLine("Operations: add, subtract, multiply, divide");

////// Demonstrate concurrent execution
////calculator.Calculate("add", 5, 3);
////calculator.Calculate("subtract", 10, 2);
////calculator.Calculate("multiply", 4, 3);
////calculator.Calculate("divide", 20, 4);

////// Keep the console open
////Console.WriteLine("Press any key to exit...");
////Console.ReadKey();
////Console.WriteLine("Multithreaded Calculator");
////Console.WriteLine("Operations: add, subtract, multiply, divide");
////Console.WriteLine("Format: operation number1 number2");
////while (true)
////{
////    Console.Write("Enter command: ");
////    string input = Console.ReadLine()!;
////    if (string.IsNullOrWhiteSpace(input))
////    {
////        break;
////    }

////    string[] parts = input.Split(' ');
////    if (parts.Length != 3)
////    {
////        Console.WriteLine("Invalid command format.");
////        continue;
////    }

////    string operation = parts[0];
////    if (!double.TryParse(parts[1], out double a) || !double.TryParse(parts[2], out double b))
////    {
////        Console.WriteLine("Invalid numbers.");
////        continue;
////    }

////    calculator.Calculate(operation, a, b);
////}
//////////////////////////////////////////////////////////////////

////ReadFile with Threading
////ReadFile read = new();
////read.GetFileFunction();
/////////////////////////////////////
/////ScrapeUrl
////ScrapeUrl scrapeUrl = new ScrapeUrl();
////await scrapeUrl.ScrapeUrlMain();
////////////////////////////////////
///ThreadedChatServer
////ChatServer client = new ChatServer();
////client.HandleClientMain();

RedisChatServer redisclient = new RedisChatServer();
redisclient.HandleRedisClientMain();
//////////////////////////////////


