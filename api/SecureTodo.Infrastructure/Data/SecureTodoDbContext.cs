using Microsoft.EntityFrameworkCore;
using SecureTodo.Domain.Task;

namespace SecureTodo.Infrastructure.Data;

public class SecureTodoDbContext : DbContext
{
    public SecureTodoDbContext(DbContextOptions<SecureTodoDbContext> options) : base(options) { }
    
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecureTodoDbContext).Assembly);
    }
}