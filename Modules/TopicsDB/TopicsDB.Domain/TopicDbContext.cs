using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using TopicDb.Domain.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace TopicDb.Domain
{
    [DbConfigurationType(typeof(TopicDbConfiguration))]
    public class TopicDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TopicDb.db");
        }

        public Microsoft.EntityFrameworkCore.DbSet<Topic> Topics { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Question> Questions { get; set; }
    }
}