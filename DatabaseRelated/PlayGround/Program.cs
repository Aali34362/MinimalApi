using Bogus;
using Dumpify;
using PlayGround;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;


/*
 * Language-Integrated Query (LINQ) is the name for a set of technologies based 
 * on the integration of query capabilities directly into the C# language
 * Query expressions are written in a declarative query syntax.
 * By using query syntax, you perform filtering, ordering, and grouping operations 
 * on data sources with a minimum of code
 * We use the same query expression patterns to query and transform 
 * data from any type of data source.
 * The variables in a query expression are all strongly typed.
 * A query isn't executed until you iterate over the query variable,
 * for example in a foreach statement.
 * 
 * At compile time, query expressions are converted to standard query 
 * operator method calls according to the rules defined in the C# specification. 
 * Any query that can be expressed by using query syntax can also be 
 * expressed by using method syntax. 
 *  In some cases, query syntax is more readable and concise. In others, method syntax is more readable. There's no semantic or performance difference between the two different forms. 
 *  Some query operations, such as Count or Max, have no equivalent query
 *  expression clause and must therefore be expressed as a method call. 
 *  Method syntax can be combined with query syntax in various ways.
 *  Query expressions can be compiled to expression trees or to delegates, 
 *  depending on the type that the query is applied to.
 *  IEnumerable<T> queries are compiled to delegates.
 *  IQueryable and IQueryable<T> queries are compiled to expression trees.
 *  
 * 
 */
IEnumerable<int> collection = [ 1, 2, 3, 4, 5, 1, 6];
IEnumerable<object> objcollection = [ 1, 2, 3, 5, 1];
IEnumerable<List<int>> listcollection = [[8, 7, 6, 4, 9],[1, 2, 3, 5]];
IEnumerable<object> obj = [1, "abc", 2, 3, 5];
IEnumerable < Person > Person = [new(1, "You", 15), new(2, "Me",16), new(1, "them",16)];
List < Person > Persons = [new(1, "You", 15), new(2, "Me",16), new(3, "them",16)];
List < Person > thenbyorderperson = [new(1, "John", 25), new(2, "John",23), new(3, "Marry",26)];
List < Product > product = [new(1, "Your"), new(2, "Men"), new(3, "then")];
IEnumerable<int> rangecollection = Enumerable.Range(0, 100);
IEnumerable<int> repeatcollection = Enumerable.Repeat(0, 100);
IEnumerable<int> emptycollection = Enumerable.Empty<int>();
IEnumerable<int> emptyarraycollection = Array.Empty<int>();
IEnumerable<int> union1 = [1, 2, 3];
IEnumerable<int> union2 = [2, 3, 4];
IEnumerable<int> sequence1 = [1, 2, 3, 4];
IEnumerable<string> stringcollection = ["a","b","c","d"];

/// Where
collection.Where(x=> x > 2).Dump();

///OfType

IEnumerable<int> obj1 = obj.OfType<int>().Dump();

IEnumerable<string> obj2 = obj.OfType<string>().Dump();

//Partitioning
obj.Skip(1).Dump();

obj.SkipLast(1).Dump();

collection.SkipWhile(x => x < 2).Dump();

obj.Take(1).Dump();

obj.TakeLast(1).Dump();

collection.TakeWhile(x => x < 2).Dump();

//Projection
collection.Select(x => x < 2).Dump();//Deferred Execution

collection.Select(x => x.ToString()).Dump();

collection.Select((x, i) => $"{i} {x}").Dump();

listcollection.SelectMany(x => x).Dump();

listcollection
    .SelectMany(x => x)
    .Select(x => x.ToString())
    .Dump();

listcollection
    .SelectMany(x => x.Select(x => x.ToString()))
    .Dump();

listcollection
    .SelectMany((x,i) => x.Select(x => $"{i} {x}"))
    .Dump();

objcollection.Cast<int>().Dump();

collection.Chunk(3).Dump();

var result = collection
    .Chunk(3)
    .SelectMany(x=>x);
//we are performing Chunk and SelectMany execution twice it gets executed only when data is iterated to display

result.Dump();
result.Dump();

var results = collection
    .Chunk(3)
    .SelectMany(x => x).ToList();
//we are performing Chunk and SelectMany execution twice it gets executed only when data is iterated to display

results.ToList().Dump();

results.ToList().Dump();



//Existence or Quantity Checks

collection.Any(x => x > 2).Dump();//True

collection.All(x => x > 2).Dump();//False

collection.Contains(2).Dump();//True

//Sequence Manipulation 
collection.Append(7).Dump();

collection.Prepend(0).Dump();

//Aggregation Methods
collection.Count().Dump();

collection.Where(x => x > 2).Count().Dump();

collection.Where(x => x > 2).TryGetNonEnumeratedCount(out var count).Dump();

collection.TryGetNonEnumeratedCount(out var count1).Dump();

collection.Max().Dump();

collection.Max(x=> x * -1).Dump();

collection.MaxBy(x=> x * -1).Dump();

Person.MaxBy(x => x.age).Dump();

collection.Min().Dump();

collection.Sum().Dump();

collection.Sum(x=> (int)x).Dump();

collection.Average().Dump();

collection.Average(x => (int)x).Dump();

collection.LongCount().Dump();

collection.Aggregate((x,y)=> x+ y).Dump();

collection
    .Select(x=> x.ToString())
    .Aggregate((x,y)=> $"{x},{y}")
    .Dump();

collection.Aggregate(10, (x, y) => x + y).Dump();

collection.Aggregate(10, (x, y) => x + y, x=> x/2).Dump();

collection
    .Aggregate(10, (x, y) => x + y, x => (float)x / collection.Count()).Dump();



//Element Operator

collection.First().Dump();

collection.FirstOrDefault().Dump();

collection.FirstOrDefault(-1).Dump();

//collection.Single().Dump();

//collection.SingleOrDefault().Dump();

collection.Last().Dump();

collection.LastOrDefault().Dump();

collection.ElementAt(1).Dump();

//collection.ElementAt(12).Dump();

collection.ElementAtOrDefault(12).Dump();

collection.DefaultIfEmpty().Dump();


////Methods

collection.ToArray().Dump();

collection.ToList().Dump();

collection.ToHashSet().Dump();

Person.ToLookup(x => x.age).Dump();
Person.ToLookup(x => x.age)[15].Single().Dump();
Person.ToLookup(x => x.age)[15].Single().Name.Dump();

//collection.ToDictionary(key => key, value => value).Dump(); // change the collection create new

////Iteration Methods

IEnumerable<Person> enumerableperson = Persons.AsEnumerable().Dump();

IQueryable<Person> queryableperson = Persons.AsQueryable().Dump();

rangecollection.Dump();

repeatcollection.Dump();

emptycollection.Dump();
emptyarraycollection.Dump();

/////Set Operators

collection.Distinct().Dump();

Person.DistinctBy(x=>x.id).Dump();

union1.Union(union2).Dump();
Person.UnionBy(Persons,  x => x.id).Dump("Union");

union1.Intersect(union2).Dump();
union1.IntersectBy(union2, x => x).Dump("Intersect");

union1.Except(union2).Dump();
union1.ExceptBy(union2, x => x).Dump("Except");

union1.SequenceEqual(union2).Dump();
union1.SequenceEqual(sequence1).Dump();

/////Join and Grouping
///

sequence1.Zip(stringcollection).Dump();
union1.Zip(stringcollection).Dump();
union1.Zip(stringcollection,union2).Dump();

Persons.Join(
    product, 
    person => person.id, 
    products => products.PersonId, 
    (person, product) => $"{person.Name} bought {product.Name}"
    ).Dump();


Persons.GroupJoin(
    product,
    person => person.id,
    products => products.PersonId,
    (person, products) => $"{person.Name} bought {string.Join(',',products)}"
    ).Dump();

union1.Concat(union2).Dump();

Person.GroupBy(x => x.age).Dump();

IGrouping <int, Person> lastGroup  = Person.GroupBy(x => x.age).Last().Dump();
lastGroup.Key.Dump();


////Sorting


objcollection.Order().Dump();

objcollection.OrderDescending().Dump();

objcollection.OrderByDescending(x => x).Dump();

objcollection.OrderBy(x=>x).Dump();

thenbyorderperson.OrderBy(x=>x.Name).ThenBy(x=>x.age).Dump();
thenbyorderperson.OrderBy(x=>x.Name).ThenByDescending(x=>x.age).Dump();

objcollection.Reverse().Dump();



//Parallel LINQ

var stopwatch = Stopwatch.StartNew();
ConcurrentDictionary<int, List<int>> threadsMap = [];

/*var HeavyComputationEnumerableRangecollection = Enumerable.Range(0, 10)
    .AsParallel()
    .WithDegreeOfParallelism(2)
    .AsOrdered()
    .AsUnordered()
    .Select(HeavyComputation);*/


/*var HeavyComputationEmptycollection = ParallelEnumerable.Empty<int>()
    .AsParallel()
    .WithDegreeOfParallelism(2)
    .AsOrdered()
    .AsUnordered()
    .Select(HeavyComputation);*/

/*var HeavyComputationRepeatcollection = ParallelEnumerable.Repeat(0, 10)
    .AsParallel()
    .WithDegreeOfParallelism(2)
    .AsOrdered()
    .AsUnordered()
    .Select(HeavyComputation);*/

var HeavyComputationRangeAsUnorderedcollection = ParallelEnumerable.Range(0, 10)
    .AsParallel()
    .WithDegreeOfParallelism(2)
    .AsUnordered()
    .Select(HeavyComputation);

var HeavyComputationRangeAsOrderedcollection = ParallelEnumerable.Range(0, 10)
    .AsParallel()
    .AsOrdered()
    .WithDegreeOfParallelism(2)    
    .Select(HeavyComputation);

foreach ( var n in HeavyComputationRangeAsOrderedcollection)
{
    n.Dump();
}

foreach (var _ in HeavyComputationRangeAsOrderedcollection) ;

stopwatch.Stop();

threadsMap.Dump();
stopwatch.ElapsedMilliseconds.Dump("Execution time");
/*int HeavyComputation(int n)
{
    $"Working on thread {Environment.CurrentManagedThreadId}".Dump();
    threadsMap.AddOrUpdate(key: Environment.CurrentManagedThreadId,
        addValue: [n],
        updateValueFactory: (keys, values) => [..values, n]);

    for (int i = 0; i < 100_000_000; i++)
    {
        n += i;
    }
    return n;
}*/

int HeavyComputation(int n)
{
    var originalN = n;
    $"Working on thread {Environment.CurrentManagedThreadId}".Dump();
    threadsMap.AddOrUpdate(key: Environment.CurrentManagedThreadId,
        addValue: [n],
        updateValueFactory: (keys, values) => [.. values, n]);

    for (int i = 0; i < 100_000_000; i++)
    {
        n += i;
    }
    return originalN;
}









////////////////////////////////////////////////////////
var enumerator = collection.GetEnumerator();
enumerator.MoveNext();
enumerator.Current.Dump();

var filteredCollection = collection.Where(x => x > 2);
foreach (var item in filteredCollection)
{
    Console.WriteLine(item);
}
var filteredCollection1 = collection.Where(x => x > 1 && x < 5);
foreach (var item in filteredCollection1)
{
    Console.WriteLine(item);
}

var filteredCollection2 = collection.Where((x, index) => index % 2 == 0);
foreach (var item in filteredCollection2)
{
    Console.WriteLine(item);
}
IEnumerable<string> collections = new List<string> { "apple", "banana", "grape", "orange" };
var filteredCollection3 = collections.Where(x => x.Contains("a"));
foreach (var item in filteredCollection3)
{
    Console.WriteLine(item);
}



//////////IEnumerable & IEnumerator////////////////
/*
 * IEnumerable and IEnumerator are interfaces in C# that are used to work with 
 * collections and enable iteration over them. 
 * They are part of the IEnumerable pattern, which allows objects to provide an 
 * iterator for traversing their elements.
 */
TestStreamReaderEnumerable();
Console.WriteLine("---");
TestReadingFile();
/*
IEnumerable:
IEnumerable is the base interface for all non-generic collections that can be enumerated.
It contains a single method, GetEnumerator(), which returns an IEnumerator interface.
It allows you to iterate over a collection using a foreach loop.
It provides read-only access to a collection.

IEnumerator:

IEnumerator provides a way to iterate over a collection one element at a time.
It contains three members:
Current: Gets the current element in the collection.
MoveNext(): Advances the enumerator to the next element of the collection. 
It returns true if there are more elements to iterate over; otherwise, it returns false.
Reset(): Sets the enumerator to its initial position, which is before the first element in the collection.

IEnumerator is disposable, so it should be disposed of after use to release resources. 
However, this is typically handled implicitly by using the foreach loop,
which calls Dispose() on the enumerator when it's no longer needed.
 */
//////////////////////////////////////////////////////////////

/*
 Deferred execution and immediate execution refer to the timing of the execution of a query or operation in LINQ.

    Deferred Execution:
        Definition: In deferred execution, the query is not executed immediately after it is created. 
Instead, it is executed only when the result is actually needed, typically when it is enumerated or when 
an operation such as ToList(), ToArray(), or foreach is performed on the query.
        Advantages: Deferred execution allows for more optimized and efficient queries because the query 
is executed only once, and the results are stored or computed as needed. This can result in better performance, 
especially when dealing with large datasets or complex queries.
        Example: Most LINQ methods, such as Where, Select, and OrderBy, use deferred execution. 
In the code you provided, methods like Where, Select, and SelectMany utilize deferred execution.

    Immediate Execution:
        Definition: In immediate execution, the query is executed as soon as it is created. 
The entire result set is computed and returned at the moment the query is invoked.
        Advantages: Immediate execution can be beneficial when you want to ensure that the query is 
executed immediately, and you want to work with the final result without worrying about deferred execution behavior.
        Example: Methods like ToList(), ToArray(), Count(), or foreach trigger immediate execution. 
For example, when you call ToList() on a query, it forces the execution of the query, and the result is stored in a list.

In your provided code snippet, methods like Dump() and certain LINQ methods like ToList() or SelectMany() can trigger 
immediate execution. For instance, when you call Dump() at the end of a query, it might cause the query to be executed 
immediately and display the results. It's important to be aware of the execution strategy, especially when working with
potentially expensive or resource-intensive operations.
 */

/*
Where
OfType
Skip
Take
SkipLast & TakeLast
SkipWhile & TakeWhile
Select
Select with index
SelectMany
SelectMany with index
Cast
Chunk
Deferred Execution vs Immediate Execution
Any
All
Contains
Append & Prepend
Count
TryGetNonEnumeratedCount
Max
MaxBy
Min & MinBy
Sum
Average
LongCount
Aggregate (overload 1)
Aggregate (overload 2)
Aggregate (overload 3)
First & FirstOrDefault
Single & SingleOrDefault
Last & LastOrDefault
ElementAt & ElementAtOrDefault
DefaultIfEmpty
ToArray
ToList
ToDictionary
ToHashSet
ToLookup
ToEnumerable & ToQueryable
Enumerable.Range
Enumerable.Repeat
Enumerable.Empty
Distinct
DistinctBy
Set Operations Theory (Union, Intersect, Except)
Union
Intersect
Except
UnionBy & IntersectBy & Except
SequenceEqual
Zip
Join
GroupJoin
Concat
GroupBy
Order
OrderBy
OrderDescending & OrderByDescending
ThenBy & ThenByDescending
Reverse
PLINQ
 */

//////////IEnumerable & IEnumerator////////////////
static void TestStreamReaderEnumerable()
{
    // Check the memory before the iterator is used.
    long memoryBefore = GC.GetTotalMemory(true);
    IEnumerable<String> stringsFound;
    // Open a file with the StreamReaderEnumerable and check for a string.
    try
    {
        stringsFound =
              from line in new StreamReaderEnumerable(@"C:\Users\aa882\Pictures\testimony.txt")
              where line.Contains("string to search for")
              select line;
        Console.WriteLine("Found: " + stringsFound.Count());
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine(@"This example requires a file named C:\Users\aa882\Pictures\testimony.txt.");
        return;
    }

    // Check the memory after the iterator and output it to the console.
    long memoryAfter = GC.GetTotalMemory(false);
    Console.WriteLine("Memory Used With Iterator = \t"
        + string.Format(((memoryAfter - memoryBefore) / 1000).ToString(), "n") + "kb");
}

static void TestReadingFile()
{
    long memoryBefore = GC.GetTotalMemory(true);
    StreamReader sr;
    try
    {
        sr = File.OpenText("C:\\Users\\aa882\\Pictures\\testimony.txt");
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine(@"This example requires a file named C:\temp\tempFile.txt.");
        return;
    }

    // Add the file contents to a generic list of strings.
    List<string> fileContents = new List<string>();
    while (!sr.EndOfStream)
    {
        fileContents.Add(sr.ReadLine());
    }

    // Check for the string.
    var stringsFound =
        from line in fileContents
        where line.Contains("string to search for")
        select line;

    sr.Close();
    Console.WriteLine("Found: " + stringsFound.Count());

    // Check the memory after when the iterator is not used, and output it to the console.
    long memoryAfter = GC.GetTotalMemory(false);
    Console.WriteLine("Memory Used Without Iterator = \t" +
        string.Format(((memoryAfter - memoryBefore) / 1000).ToString(), "n") + "kb");
}
//////////////////////////////////////////////////////////////


record Person(int id, string Name, int age);

record Product(int PersonId, string Name);