using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZooAppUsingSp.Models;

namespace ZooAppUsingSp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TestUnit> TestUnits { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<ClientHeader> ClientHeaders { get; set; }
    }
}