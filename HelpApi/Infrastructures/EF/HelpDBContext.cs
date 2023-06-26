using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructures.EF.HelpDB
{
    public partial class HelpDBContext : DbContext
    {
        public HelpDBContext()
        {
        }

        public HelpDBContext(DbContextOptions<HelpDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Guide> Guide { get; set; }
        public virtual DbSet<GuideCate> GuideCate { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.GuideCode)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasComment("");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GuideCate>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.Title).HasMaxLength(512);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
