using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HelpApi.EF
{
    public partial class MasterDBContext : DbContext
    {
        public MasterDBContext()
        {
        }

        public MasterDBContext(DbContextOptions<MasterDBContext> options)
            : base(options)
        {
        }
        //public virtual DbSet<Role> Role { get; set; }
       // public virtual DbSet<RoleDataPermission> RoleDataPermission { get; set; }
      //  public virtual DbSet<RolePermission> RolePermission { get; set; }
       // public virtual DbSet<Subscription> Subscription { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new { e.SubsidiaryId, e.UserName }, "IX_User_UserName")
                    .IsUnique()
                    .HasFilter("([IsDeleted]=(0) AND [UserName]<>'' AND [UserName] IS NOT NULL)");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.AccessFailedCount).HasComment("");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedDatetimeUtc).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
