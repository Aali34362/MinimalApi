namespace GraphQLProject.Models;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Crtd_User { get; set; } = "Admin";
    public DateTime? Crtd_Dt { get; set; } = DateTime.Now;
    public string? Lst_Crtd_User { get; set; } = "Admin";
    public DateTime? Lst_Crtd_Dt { get; set; } = DateTime.Now;
    public int Actv_Ind { get; set; } = 1;
    public bool Del_Ind { get; set; } = false;
}
