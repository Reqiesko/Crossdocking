
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                //.UseLazyLoadingProxies()
                .UseSqlite("Data Source=Users.db")
                .EnableSensitiveDataLogging(true)
                .LogTo(s => Debug.WriteLine(s));
        }
    }
}
