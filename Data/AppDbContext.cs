using Microsoft.EntityFrameworkCore;
using PalaganasTechnicalExam.Models.Entities;

namespace PalaganasTechnicalExam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
