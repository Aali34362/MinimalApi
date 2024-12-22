using Microsoft.EntityFrameworkCore;
using PizzaOrder.Data.Entities;

namespace PizzaOrder.Data.Context;

public class PizzaDbContext(DbContextOptions<PizzaDbContext> options) : DbContext(options)
{
    public DbSet<PizzaDetails> PizzaDetails { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
}
