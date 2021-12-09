using Microsoft.EntityFrameworkCore;
using PowerTools.Models;

namespace PowerTools.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options): base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
}
