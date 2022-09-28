using Microsoft.EntityFrameworkCore;
using MovieStore.Catalog.Common.Models;

namespace MovieStore.Catalog.Repository
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Movie> Movies { get; set; } = null!;
    }
}
