namespace MongoDBDemo.Responses;

public sealed class PaginatedList<T>
{
    public IList<T>? Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / (double)PageSize);
    public bool IsNullOrEmpty() => Items == null || Items.Any();
    
}

public static class PaginatedListExtensions
{
    public static PaginatedList<T> ToPaginatedList<T>(this IList<T> source, int pageNumber, int pageSize, int total)
    {
        return new PaginatedList<T>
        {
            Items = source,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total
        };
    }
}
