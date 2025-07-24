using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Backend.Database
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<About> AboutSection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<ShortUrl>()
                .HasIndex(u => u.ShortCode)
                .IsUnique();

            modelBuilder.Entity<ShortUrl>()
                .HasOne(u => u.CreatedBy)
                .WithMany(u => u.ShortUrls)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
