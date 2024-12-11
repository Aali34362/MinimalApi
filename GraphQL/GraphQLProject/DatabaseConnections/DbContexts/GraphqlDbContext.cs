using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GraphQLProject.DatabaseConnections.DbContexts;

public class GraphqlDbContext(DbContextOptions<GraphqlDbContext> options) : DbContext(options)
{
    // DbSet for the Menu entity
    public DbSet<Menu>? Menus { get; set; }
    public DbSet<Category>? Category { get; set; }
    public DbSet<Reservation>? Reservations { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Configure default max length for all strings
        configurationBuilder.Properties<string>().HaveMaxLength(100); // Default string length
        // Configure default precision for decimals
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);// Default precision for decimal
    }

    // Override SaveChangesAsync to handle custom logic (e.g., auditing)
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Example: Add custom logic here (e.g., audit fields, validation, etc.) 
        return await base.SaveChangesAsync(cancellationToken);
    }

    // Override OnConfiguring to ensure the connection string is set for migrations
    ////protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    ////{
    ////    if (!optionsBuilder.IsConfigured)
    ////    {
    ////        optionsBuilder.UseSqlServer("Server=AMIR_ALI\\SQLEXPRESS;Database=GraphqlDB;User=Amir_Ali\\Admin;Password=; MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=true;Connection Timeout=6000;Trusted_Connection=True;"); // Provide connection string here
    ////    }
    ////}

    // Apply entity configurations using the Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations in the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // Optional: Additional custom model configurations can go here
    }

}
