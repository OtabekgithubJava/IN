using JobBoard.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Persistance
{
    public class JobBoardDbContext : DbContext
    {
        public JobBoardDbContext(DbContextOptions<JobBoardDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }
}