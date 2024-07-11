//Console.WriteLine("Press any key to cancel the operation...");
//var longRunningTask = LongRunningOperationAsync();

//Console.ReadKey();
//Console.WriteLine("Key Pressed");

//async Task LongRunningOperationAsync()
//{
//    var i = 0;
//    while (i++ < 10)
//    {
//        Console.WriteLine($"Working {i}");
//        await Task.Delay(2000);
//        Console.WriteLine($"Completed {i}");
//    }
//}


using cancellation_token_example;
using Dumpify;
using System.Globalization;

//var source = new CancellationTokenSource();
//Console.WriteLine("Press any key to cancel the operation...");
//var longRunningTask = LongRunningOperationAsync(source.Token);

//Console.ReadKey();
//Console.WriteLine("Key Pressed");
//source.Cancel();

//await longRunningTask;

//async Task LongRunningOperationAsync(CancellationToken cancellationToken)
//{
//    try
//    {
//        var i = 0;
//        while (i++ < 10)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            // Simulate some work
//            Console.WriteLine($"Working {i}");
//            await Task.Delay(2000, cancellationToken);
//            Console.WriteLine($"Completed {i}");
//        }
//    }
//    catch (OperationCanceledException)
//    {
//        Console.WriteLine("User cancelled the operation");
//    }
//}

////////////////////////////////////////////////////////////
//await Console.Out.WriteLineAsync();
//await Console.Out.WriteLineAsync(new string('-',40));
//await Console.Out.WriteLineAsync("Cancellation Token Example Form MS");

//CancellationTokenSource sources = new CancellationTokenSource();
//CancellationToken token = sources.Token;

//sources.Dump();
//token.Dump();

//Random rnd = new Random();
//Object lockObj = new Object();

//List<Task<int[]>> tasks = new List<Task<int[]>>();
//TaskFactory factory = new TaskFactory(token);
//for (int taskCtr = 0; taskCtr <= 10; taskCtr++)
//{
//    int iteration = taskCtr + 1;
//    factory.Dump();
//    tasks.Add(factory.StartNew(() => {
//        int value;
//        int[] values = new int[10];
//        for (int ctr = 1; ctr <= 10; ctr++)
//        {
//            lock (lockObj)
//            {
//                value = rnd.Next(0, 101);
//            }
//            if (value == 0)
//            {
//                sources.Cancel();
//                Console.WriteLine("Cancelling at task {0}", iteration);
//                break;
//            }
//            values[ctr - 1] = value;
//        }
//        return values;
//    }, token));
//}
//try
//{
//    tasks.Dump();
//    Task<double> fTask = factory.ContinueWhenAll(tasks.ToArray(),
//    (results) => {
//        Console.WriteLine("Calculating overall mean...");
//        long sum = 0;
//        int n = 0;
//        foreach (var t in results)
//        {
//            foreach (var r in t.Result)
//            {
//                sum += r;
//                n++;
//            }
//        }
//        return sum / (double)n;
//    }, token);
//    Console.WriteLine("The mean is {0}.", fTask.Result);
//}
//catch (AggregateException ae)
//{
//    foreach (Exception e in ae.InnerExceptions)
//    {
//        if (e is TaskCanceledException)
//            Console.WriteLine("Unable to compute mean: {0}",
//               ((TaskCanceledException)e).Message);
//        else
//            Console.WriteLine("Exception: " + e.GetType().Name);
//    }
//}
//finally
//{
//    sources.Dispose();
//}

//////////////////////////////////////////////////
//await Console.Out.WriteLineAsync();
//await Console.Out.WriteLineAsync(new string('-', 40));
//await Console.Out.WriteLineAsync("Cancellation Token Example Form MS");

//// The Simple class controls access to the token source.
//CancellationTokenSource cts = new CancellationTokenSource();

//Console.WriteLine("Press 'C' to terminate the application...\n");
//// Allow the UI thread to capture the token source, so that it
//// can issue the cancel command.
//Thread t1 = new Thread(() => {
//    if (Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant() == "C")
//        cts.Cancel();
//});

//// ServerClass sees only the token, not the token source.
//Thread t2 = new Thread(new ParameterizedThreadStart(ServerClass.StaticMethod));
//// Start the UI thread.

//t1.Start();

//// Start the worker thread and pass it the token.
//t2.Start(cts.Token);

//t2.Join();
//cts.Dispose();



//////////////////////////////////////////////////
await Console.Out.WriteLineAsync();
await Console.Out.WriteLineAsync(new string('-', 40));
await Console.Out.WriteLineAsync("Prime");

var range = GetRange();

while (range.Lo > 0 && range.Hi > 0)
{
    try
    {
        var ctc = new CancellationTokenSource();

        ctc.Cancel();

        Task<int>[] tasks =
        {
            Task.Run(() => PrimeCount(range.Lo, range.Hi,ctc.Token),ctc.Token),
            Task.Run(() => PrimeCount(range.Lo, range.Hi,ctc.Token),ctc.Token),
            Task.Run(() => PrimeCount(range.Lo, range.Hi,ctc.Token),ctc.Token)
        };

        ReportTasks(tasks);

        var finisher = await Task.WhenAny(tasks);

        ReportTasks(tasks);

        ctc.Cancel();

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        ReportTasks(tasks);

        Console.WriteLine();
        Console.WriteLine($"The calculation was done by algorithm number {Array.IndexOf(tasks, finisher)}");
        Console.WriteLine($"There are {finisher.Result:N0} primes between {range.Lo:N0} and {range.Hi:N0}.");
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    range = GetRange();
}

int PrimeCount(ulong lo, ulong hi, CancellationToken token)
{
    int count = 0;

    var root = Math.Sqrt(hi);
    var rand = new Random();

    var primes = PrimeList.Primes.TakeWhile(p => p <= root).OrderBy(x => rand.Next()).ToArray();

    for (ulong num = lo; num <= hi; ++num)
    {
        token.ThrowIfCancellationRequested();

        if (IsPrime(num))
            ++count;
    }

    return count;

    bool IsPrime(ulong num)
    {
        foreach (ulong x in primes)
        {
            if (num % x == 0)
                return false;
        }

        return true;
    }
}

(ulong Lo, ulong Hi) GetRange()
{
    Console.Write("Lower bound: ");
    var lo = ulong.Parse(Console.ReadLine() ?? "", NumberStyles.AllowThousands);

    Console.Write("Upper bound: ");
    var hi = ulong.Parse(Console.ReadLine() ?? "", NumberStyles.AllowThousands);

    return (lo, hi);
}

void ReportTasks(Task<int>[] tasks)
{
    for (int i = 0; i < tasks.Length; ++i)
    {
        Console.WriteLine($"Task: {i}");
        Console.WriteLine($"IsCompleted: {tasks[i].IsCompleted}");
        Console.WriteLine($"IsCompletedSuccessfully: {tasks[i].IsCompletedSuccessfully}");
        Console.WriteLine($"IsCanceled: {tasks[i].IsCanceled}");
        Console.WriteLine($"IsFaulted: {tasks[i].IsFaulted}");
        Console.WriteLine();
    }
}