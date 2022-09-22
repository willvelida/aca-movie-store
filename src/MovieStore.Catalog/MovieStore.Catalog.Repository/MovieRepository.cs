using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Repository.Interfaces;

namespace MovieStore.Catalog.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;
        private readonly ILogger _logger;

        public MovieRepository(MovieContext context, ILogger<MovieRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            try
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(AddMovieAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(DeleteMovieAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetMovieAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            try
            {
                var movies = await _context.Movies
                    .ToListAsync();

                return movies;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetMoviesAsync)}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            try
            {
                _context.Entry(movie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(UpdateMovieAsync)}: {ex.Message}");
                throw;
            }
        }
    }
}
