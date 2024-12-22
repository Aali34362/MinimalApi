using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaOrder.Data.Entities;

public class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Crtd_User { get; set; } = "Admin";
    public DateTime? Crtd_Dt { get; set; } = DateTime.Now;
    public string? Lst_Crtd_User { get; set; } = "Admin";
    public DateTime? Lst_Crtd_Dt { get; set; } = DateTime.Now;
    public int Actv_Ind { get; set; } = 1;
    public bool Del_Ind { get; set; } = false;
}
