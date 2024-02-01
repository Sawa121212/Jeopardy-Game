using System.Data.Entity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Users.Domain
{
    [DbConfigurationType(typeof(UserDbConfiguration))]
    public sealed class UserDbContext : DbContext
    {
        public UserDbContext()
        {
            // при создании контекста автоматически проверит наличие базы данных и, если она отсутствует, создаст ее.
            bool isCreated = Database.EnsureCreated();
            Debug.WriteLine(isCreated ? "База данных была создана" : "База данных уже существует");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=UserDb.db");
        }

        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
    }
}
