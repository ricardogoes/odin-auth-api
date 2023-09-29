using Microsoft.EntityFrameworkCore;
using Odin.Auth.Infra.Data.EF.Configurations;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.Infra.Data.EF
{
    public class OdinMasterDbContext : DbContext
    {
        public DbSet<CustomerModel> Customers { get; set; }

        public OdinMasterDbContext(DbContextOptions<OdinMasterDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
