using GrpcServer.Model;
using Microsoft.EntityFrameworkCore;

namespace GrpcServer.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
}
