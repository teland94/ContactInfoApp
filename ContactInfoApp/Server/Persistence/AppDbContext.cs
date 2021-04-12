using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Server.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContactInfoApp.Server.Persistence
{
    public class AppDbContext : DbContext
    {
        private DatabaseSettings DatabaseSettings { get; }

        public DbSet<SearchContactHistory> SearchContactHistory { get; set; }

        public AppDbContext(IOptions<DatabaseSettings> databaseSettingsAccessor)
        {
            DatabaseSettings = databaseSettingsAccessor.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchContactHistory>().Property(p => p.PhoneNumber).IsRequired();
            modelBuilder.Entity<SearchContactHistory>().Property(p => p.DisplayName).IsRequired();
        }
    }
}
