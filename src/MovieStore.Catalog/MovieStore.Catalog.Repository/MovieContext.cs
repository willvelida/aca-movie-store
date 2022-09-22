using Microsoft.EntityFrameworkCore;
using MovieStore.Catalog.Common.Models;

namespace MovieStore.Catalog.Repository
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
    }
}
