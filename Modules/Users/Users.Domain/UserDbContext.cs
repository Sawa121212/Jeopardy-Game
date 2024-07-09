using System;
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
            if (Database is null)
            {
                Debug.WriteLine($"[Debug] {nameof(UserDbContext)}: База данных не найдена");
                return;
            }

            bool isCreated = false;

            try
            {
                // при создании контекста автоматически проверит наличие базы данных и, если она отсутствует, создаст ее.
                isCreated = Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Debug] {nameof(UserDbContext)}: База данных^ Exception: {e.Message}");
                return;
            }

            Debug.WriteLine(isCreated
                ? $"[Debug] {nameof(UserDbContext)}: База данных была создана"
                : $"[Debug] {nameof(UserDbContext)}: База данных уже существует");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Environment.CurrentDirectory}/UserDb.db");
        }

        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
    }
}