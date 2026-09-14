using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Movie.Domain.Entities;

namespace Movie.Infrastructure.Data
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie.Domain.Entities.Movie> Movies { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<StudioDetails> StudioDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=mamusisha;Database=MovieDB;Trusted_Connection=True; TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Country - Studio (One-to-Many)
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Studios)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId);

            modelBuilder.Entity<Country>().Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Studio - StudioDetails (One-to-One)
            modelBuilder.Entity<Studio>()
                .HasOne(s => s.StudioDetails)
                .WithOne(sd => sd.Studio)
                .HasForeignKey<StudioDetails>(sd => sd.StudioId);

            modelBuilder.Entity<Studio>().Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<StudioDetails>().Property(sd => sd.LicenseNumber)
                .IsRequired();

            // Studio - Movie (One-to-Many)
            modelBuilder.Entity<Studio>()
                .HasMany(s => s.Movies)
                .WithOne(m => m.Studio)
                .HasForeignKey(m => m.StudioId);

            modelBuilder.Entity<Movie.Domain.Entities.Movie>().Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            // Movie - Actor (Many-to-Many, named join table)
            modelBuilder.Entity<Movie.Domain.Entities.Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.ToTable("MovieActors"));

            modelBuilder.Entity<Actor>().Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Actor>().Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Seed data
            modelBuilder.Entity<Country>()
                .HasData(
                    new Country { Id = 1, Name = "USA" },
                    new Country { Id = 2, Name = "UK" },
                    new Country { Id = 3, Name = "France" }
                );

        }
    }
}
