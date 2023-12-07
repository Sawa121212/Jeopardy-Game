using System;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TopicDb.Domain.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace TopicDb.Domain
{
    [DbConfigurationType(typeof(TopicDbConfiguration))]
    public sealed class TopicDbContext : DbContext
    {
        public TopicDbContext()
        {
            // при создании контекста автоматически проверит наличие базы данных и, если она отсутствует, создаст ее.
            bool isCreated = Database.EnsureCreated();
            Debug.WriteLine(isCreated ? "База данных была создана" : "База данных уже существует");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TopicDb.db");
        }

        public Microsoft.EntityFrameworkCore.DbSet<Topic> Topics { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Question> Questions { get; set; }
    }
}