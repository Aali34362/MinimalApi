using Microsoft.EntityFrameworkCore;
using WebHook.Receiver.Api.Models;

namespace WebHook.Receiver.Api.Context;

public class WebhookReceiverContext(DbContextOptions<WebhookReceiverContext> options) : DbContext(options)
{
    public DbSet<WebhookData> Webhooks { get; set; }
}
