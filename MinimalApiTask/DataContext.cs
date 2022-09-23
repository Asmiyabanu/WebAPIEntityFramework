global using Microsoft.EntityFrameworkCore;

namespace MinimalApiTask
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<Employee> Employees => Set<Employee>();
    }
}
