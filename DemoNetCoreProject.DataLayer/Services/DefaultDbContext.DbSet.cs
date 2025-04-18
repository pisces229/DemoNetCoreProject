using DemoNetCoreProject.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Services
{
    public partial class DefaultDbContext
    {
        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Row)
                    .HasName("PK__Address");

                entity.ToTable("Address");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Text).HasMaxLength(100);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Addresses)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__Address__Id");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Row)
                    .HasName("PK__Person");

                entity.ToTable("Person");

                entity.HasIndex(e => e.Id, "IDX__Person__Id")
                    .IsUnique();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(36);

                entity.Property(e => e.Remark).HasMaxLength(100);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
