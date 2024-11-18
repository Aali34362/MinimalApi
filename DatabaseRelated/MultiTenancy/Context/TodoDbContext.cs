using Microsoft.EntityFrameworkCore;
using MultiTenancy.Controllers;

namespace MultiTenancy.Context;

public class TodoDbContext(DbContextOptions<TodoDbContext> options, ITenant tenant) : DbContext(options)
{
    private readonly ITenant _tenant = tenant;

    public DbSet<TodoItem> Todos => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<TodoItem>().HasQueryFilter(t => t.TenantId == _tenant.Id);

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<TodoItem>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = _tenant.Id;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

public class TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public int TenantId { get; set; }
}