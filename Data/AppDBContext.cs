using Microsoft.EntityFrameworkCore;
using TestTask.Data.Models;

namespace TestTask
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public AppDBContext() : base(){ }

        public DbSet<Node> Nodes { get; set; }

    }
}
