using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Exotic.WebHook.Api.Domain.Response;

public class BaseResponse
{
    public Guid Id { get; set; }
    public string? Lst_Crtd_Usr { get; set; }
    public DateTime Lst_Crtd_Dt { get; set; }
    public short Actv_Ind { get; set; }
}