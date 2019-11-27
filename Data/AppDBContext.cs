using Microsoft.EntityFrameworkCore;

namespace TestTask
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Node> Nodes { get; set; }

    }
}
