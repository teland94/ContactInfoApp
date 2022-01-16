using System.Diagnostics;
using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Server.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContactInfoApp.Server.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        private DatabaseSettings DatabaseSettings { get; }

        public DbSet<SearchContactHistory> SearchContactHistory { get; set; }
        public DbSet<SearchContactHistoryComment> SearchContactHistoryComments { get; set; }

        public AppDbContext(IOptions<DatabaseSettings> databaseSettingsAccessor)
        {
            DatabaseSettings = databaseSettingsAccessor.Value;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseSettings.ConnectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchContactHistory>().Property(p => p.PhoneNumber).IsRequired();
            modelBuilder.Entity<SearchContactHistory>().Property(p => p.DisplayName).IsRequired();

            modelBuilder.Entity<SearchContactHistoryComment>().Property(p => p.Body).IsRequired();
        }
    }
}
