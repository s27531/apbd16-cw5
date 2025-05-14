using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: Sample data.
        base.OnModelCreating(modelBuilder);
    }
}