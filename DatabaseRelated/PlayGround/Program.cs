// See https://aka.ms/new-console-template for more information
using Dumpify;
using System.Data.SqlTypes;

IEnumerable<int> collection = [ 1, 2, 3, 4, 5, 1, 6];
IEnumerable<object> objcollection = [ 1, 2, 3, 5, 1];
IEnumerable<List<int>> listcollection = [[8, 7, 6, 4, 9],[1, 2, 3, 5]];
IEnumerable<object> obj = [1, "abc", 2, 3, 5];
IEnumerable < Person > Person = [new("You", 15), new("Me",16), new("them",16)];


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

collection.Single().Dump();

collection.SingleOrDefault().Dump();

collection.Last().Dump();

collection.LastOrDefault().Dump();

collection.ElementAt(1).Dump();

collection.ElementAt(12).Dump();

collection.ElementAtOrDefault(12).Dump();

collection.DefaultIfEmpty().Dump();


////Methods

collection.ToArray().Dump();

collection.ToList().Dump();

collection.ToHashSet().Dump();

Person.ToLookup(x => x.age).Dump();
Person.ToLookup(x => x.age)[15].Single().Dump();
Person.ToLookup(x => x.age)[15].Single().Name.Dump();

collection.ToDictionary(key => key, value => value).Dump();

////Iteration Methods










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

record Person(string Name, int age);




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

