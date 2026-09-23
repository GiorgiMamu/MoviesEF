using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie.Domain.Entities;
using MovieEntity = Movie.Domain.Entities.Movie;

namespace Movie.Infrastructure.Data;

public class MovieDbContext : DbContext
{
    public DbSet<MovieEntity> Movies { get; set; } = null!;
    public DbSet<Studio> Studios { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<StudioDetails> StudioDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(c => c.Studios)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId);

            entity.HasData(
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "UK" },
                new Country { Id = 3, Name = "France" });
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(s => s.StudioDetails)
                .WithOne(sd => sd.Studio)
                .HasForeignKey<StudioDetails>(sd => sd.StudioId);

            entity.HasMany(s => s.Movies)
                .WithOne(m => m.Studio)
                .HasForeignKey(m => m.StudioId);
        });

        modelBuilder.Entity<StudioDetails>()
            .Property(sd => sd.LicenseNumber)
            .IsRequired();

        modelBuilder.Entity<MovieEntity>(entity =>
        {
            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.ToTable("MovieActors"));
        });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}