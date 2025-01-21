using Microsoft.EntityFrameworkCore;
using WebHook.Api.Models;

namespace WebHook.Api.Data;

internal sealed class WebHooksDbContext(DbContextOptions<WebHooksDbContext> options) : DbContext(options)
{
    public DbSet<Orders> Orders { get; set; }
    public DbSet<WebHookSubscription> Subscriptions { get; set; }
    public DbSet<WebHookDeliveryAttempt> DeliveryAttempt { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Orders>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.CreatedAt)
            .HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<WebHookSubscription>(builder =>
        {
            builder.ToTable("subscriptions", "webhooks");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.CreatedAt)
            .HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<WebHookDeliveryAttempt>(builder =>
        {
            builder.ToTable("deliveryAttempt", "webhooks");
            builder.HasKey(o => o.Id);
            builder.HasOne<WebHookSubscription>()
            .WithMany()
            .HasForeignKey(d=> d.SubscriptionId);
            builder.Property(o => o.Timestamp)
            .HasColumnType("timestamp with time zone");
        });
    }
}
