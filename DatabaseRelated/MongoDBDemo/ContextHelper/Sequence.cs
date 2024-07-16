namespace MongoDBDemo.ContextHelper;

public class Sequence
{
    public string? Id { get; set; }
    public int Seq { get; set; }
}

public class SequenceParam
{
    public string? sequenceName { get; set; }
    public string? prefix { get; set; }
    public int startWith { get; set; }
    public int incrementBy { get; set; }
    public int initialPadding { get; set; }
}