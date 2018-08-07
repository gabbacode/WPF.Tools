using Data.Entities.Common.LocalBase;
using Microsoft.EntityFrameworkCore;

namespace Data.Sources.LocalStorage.Sqlite.Context
{
    public class SqliteContext : DbContext
    {
        private string _baseConnstr;
        public SqliteContext(string baseConnstr) 
        {
            _baseConnstr = baseConnstr;
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }

        public DbSet<RowInfo> Row { get; set; }
        public DbSet<ColumnInfo> Column { get; set; }
        public DbSet<BoardInfo> Board { get; set; }
        public DbSet<SqliteIssue> Issue    { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
            builder.UseSqlite(_baseConnstr);
        }
    }
}
