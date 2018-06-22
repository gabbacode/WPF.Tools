using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Common.Redmine;
using Microsoft.EntityFrameworkCore;

namespace Data.Sources.LocalStorage.Sqlite.Context
{
    public class SqliteContext : DbContext
    {
        public SqliteContext() // : base(new ("Data Source=testdb2.db; Version=3;"), true)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Tracker>     Tracker  { get; set; }
        public DbSet<Status>      Status   { get; set; }
        public DbSet<Project>     Project  { get; set; }
        public DbSet<Priority>    Priority { get; set; }
        public DbSet<SqliteIssue> Issue    { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
            builder.UseSqlite(ConfigurationManager.AppSettings["LocaBaseConnectionString"]);
        }
    }
}
