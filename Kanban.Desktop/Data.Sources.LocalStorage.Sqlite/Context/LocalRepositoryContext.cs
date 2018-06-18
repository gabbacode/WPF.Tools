using System;
using System.Data.Entity;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SQLite;
using System.Linq;
using Data.Entities.Common.Redmine;

namespace Data.Sources.LocalStorage.Sqlite.Context
{
    public class LocalRepositoryContext : DbContext
    {
        public LocalRepositoryContext() : base(new SQLiteConnection("Data Source=testdb2.db; Version=3;"),true)
        {
        }

        public DbSet<User>     User     { get; set; }
        public DbSet<Tracker>  Tracker  { get; set; }
        public DbSet<Status>   Status   { get; set; }
        public DbSet<Project>  Project  { get; set; }
        public DbSet<Priority> Priority { get; set; }
        // public DbSet<Issue>    Issue    { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //builder.Entity<Issue>().Map(delegate(EntityMappingConfiguration<Issue> issueconf)
            //{
            //    issueconf.Properties(pr=>new
            //    {
            //        pr.Id,
            //        UserId =pr.AssignedTo.Id,
            //        ProjectId=pr.Project.Id,
            //        StatusId =pr.Status.Id,
            //        PriorityId=pr.Priority.Id,
            //        TrackerId=pr.Tracker.Id,
            //        pr.Subject,
            //        pr.Description,
            //        pr.CreatedOn
            //    });
            //    issueconf.ToTable("Issue");
            //});

            //void Issmap(EntityMappingConfiguration<Issue> m)
            //{
            //}

            //builder.Entity<Issue>().HasKey(i=>i.Id)
            //    .HasRequired(i => i.AssignedTo).WithMany()
            //    .WillCascadeOnDelete();

            //builder.Entity<Issue>()
            //    .HasRequired(i=>i.Project).WithMany()
            //    .WillCascadeOnDelete();

            //builder.Entity<Issue>()
            //    .HasRequired(i => i.Status).WithMany()
            //    .WillCascadeOnDelete();

            //builder.Entity<Issue>()
            //    .HasRequired(i => i.Priority).WithMany()
            //    .WillCascadeOnDelete();

            //builder.Entity<Issue>()
            //    .HasRequired(i => i.Tracker).WithMany()
            //    .WillCascadeOnDelete();

            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User").HasKey(u => u.Id).Property(u=>u.Name).IsRequired();
            builder.Entity<User>().Property(u => u.Id).IsConcurrencyToken();

            builder.Entity<Project>().ToTable("Project").HasKey(p => p.Id);

            builder.Entity<Status>().ToTable("Status").HasKey(s => s.Id);

            builder.Entity<Priority>().ToTable("Priority").HasKey(p => p.Id);

            builder.Entity<Tracker>().ToTable("Tracker").HasKey(t => t.Id);
        }
    }
}
