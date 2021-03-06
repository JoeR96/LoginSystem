using LoginSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginSystem.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {
        }

        public DataContext()
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
