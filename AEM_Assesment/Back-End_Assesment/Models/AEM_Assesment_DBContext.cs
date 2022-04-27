using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Back_End_Assesment.Models
{
    public partial class AEM_Assesment_DBContext : DbContext
    {
        public AEM_Assesment_DBContext()
        {
        }

        public AEM_Assesment_DBContext(DbContextOptions<AEM_Assesment_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Platform> Platforms { get; set; } = null!;
        public virtual DbSet<Well> Wells { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=UZAID;Initial Catalog=AEM_Assesment_DB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("longitude");

                entity.Property(e => e.UniqueName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("uniqueName");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");
            });

            modelBuilder.Entity<Well>(entity =>
            {
                entity.ToTable("Well");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("longitude");

                entity.Property(e => e.PlatformId).HasColumnName("platformId");

                entity.Property(e => e.UniqueName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("uniqueName");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedAt");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Well)
                    .HasForeignKey(d => d.PlatformId)
                    .HasConstraintName("FK_WellTable_PlatformTable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
