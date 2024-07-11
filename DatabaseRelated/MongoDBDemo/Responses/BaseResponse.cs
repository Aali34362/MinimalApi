namespace MongoDBDemo.Responses;


public class BaseResponse
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Lst_Crtd_Usr { get; set; }
    public DateTime Lst_Crtd_Dt { get; set; }
    [DefaultValue(1)]
    public short Actv_Ind { get; set; }
}
