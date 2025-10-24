using Microsoft.EntityFrameworkCore;
using IMDB.DataServiceLayer.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace IMDB.DataServiceLayer;

public class ImdbContext : DbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=imdb;Username=postgres;Password=root");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().ToTable("appuser");
        modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        modelBuilder.Entity<AppUser>().Property(u => u.Id).HasColumnName("user_id");
        modelBuilder.Entity<AppUser>().Property(u => u.Email).HasColumnName("email");
        modelBuilder.Entity<AppUser>().Property(u => u.Username).HasColumnName("username");
        modelBuilder.Entity<AppUser>().Property(u => u.Password).HasColumnName("password");
        modelBuilder.Entity<AppUser>().Property(u => u.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<AppUser>().Property(u => u.LastLoginAt).HasColumnName("last_login_at");
        modelBuilder.Entity<AppUser>().Property(u => u.Status).HasColumnName("status");
        
        modelBuilder.Entity<Movie>().ToTable("movie");
        modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        modelBuilder.Entity<Movie>().Property(m => m.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<Movie>().Property(m => m.TitleType).HasColumnName("title_type");
        modelBuilder.Entity<Movie>().Property(m => m.PrimaryTitle).HasColumnName("primary_title");
        modelBuilder.Entity<Movie>().Property(m => m.OriginalTitle).HasColumnName("original_title");
        modelBuilder.Entity<Movie>().Property(m => m.IsAdult).HasColumnName("is_adult");
        modelBuilder.Entity<Movie>().Property(m => m.StartYear).HasColumnName("start_year");
        modelBuilder.Entity<Movie>().Property(m => m.EndYear).HasColumnName("end_year");
        modelBuilder.Entity<Movie>().Property(m => m.RunTimeMinutes).HasColumnName("runtime_minutes");
        modelBuilder.Entity<Movie>().Property(m => m.EndYear).HasColumnName("plot_summary");
        modelBuilder.Entity<Movie>().Property(m => m.EndYear).HasColumnName("poster_url");
    }
    
    
}