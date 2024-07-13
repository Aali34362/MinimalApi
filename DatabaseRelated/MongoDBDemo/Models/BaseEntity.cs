namespace MongoDBDemo.Models;

public class BaseEntity
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Crtd_Usr { get; set; } = "Admin";
    public DateTime Crtd_Dt { get; set; } = DateTime.Now;
    public string? Lst_Crtd_Usr { get; set; } = "Admin";
    public DateTime Lst_Crtd_Dt { get; set; } = DateTime.Now;
    public short Actv_Ind { get; set; } = 1;
    public short Del_Ind { get; set; } = 0;
}
