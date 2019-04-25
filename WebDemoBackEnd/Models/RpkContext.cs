using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebDemoBackEnd.Models
{
    public partial class RpkContext : DbContext
    {
        public RpkContext()
        {
        }

        public RpkContext(DbContextOptions<RpkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Entries> Entries { get; set; }
        public virtual DbSet<FoodCategories> FoodCategories { get; set; }
        public virtual DbSet<Foods> Foods { get; set; }
        public virtual DbSet<TimesOfDay> TimesOfDay { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DEEPBLUE\\SQLEXPRESS;Database=Rpk;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Entries>(entity =>
            {
                entity.HasKey(e => e.EntryId);

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.Entries)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Food_EntryID");

                entity.HasOne(d => d.TimeOfDayNavigation)
                    .WithMany(p => p.Entries)
                    .HasForeignKey(d => d.TimeOfDay)
                    .HasConstraintName("Times_EntryID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Entries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_EntryID");
            });

            modelBuilder.Entity<FoodCategories>(entity =>
            {
                entity.HasKey(e => e.FoodCategoryId)
                    .HasName("PK_Table_2");

                entity.Property(e => e.FoodCategoryId).HasColumnName("FoodCategoryID");

                entity.Property(e => e.FoodCategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Foods>(entity =>
            {
                entity.HasKey(e => e.FoodId)
                    .HasName("PK_Table1");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.AddedUserId).HasColumnName("AddedUserID");

                entity.Property(e => e.FoodCategoryId).HasColumnName("FoodCategoryID");

                entity.Property(e => e.FoodName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Kcal).HasColumnName("kcal");

                entity.HasOne(d => d.AddedUser)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.AddedUserId)
                    .HasConstraintName("fk_fooduser");

                entity.HasOne(d => d.FoodCategory)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.FoodCategoryId)
                    .HasConstraintName("Food_FoodCategoryID");
            });

            modelBuilder.Entity<TimesOfDay>(entity =>
            {
                entity.HasKey(e => e.TimeOfDay);

                entity.Property(e => e.NameOfTime)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Table_1");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });
        }
    }
}
