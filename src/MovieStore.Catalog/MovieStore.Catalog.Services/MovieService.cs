using Microsoft.Extensions.Logging;
using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Repository.Interfaces;
using MovieStore.Catalog.Services.Interfaces;

namespace MovieStore.Catalog.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger _logger;


        public MovieService(IMovieRepository movieRepository, ILogger<MovieService> logger)
        {
            _movieRepository=movieRepository;
            _logger = logger;
        }

        public async Task CreateMovie(Movie movie)
        {
            try
            {
                await _movieRepository.AddMovieAsync(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(CreateMovie)}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteMovie(int id)
        {
            try
            {
                var movieToDelete = await _movieRepository.GetMovieAsync(id);

                if (movieToDelete is null)
                {
                    throw new Exception("Movie not found");
                }

                await _movieRepository.DeleteMovieAsync(movieToDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(DeleteMovie)}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            try
            {
                var movies = await _movieRepository.GetMoviesAsync();

                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetAllMovies)}: {ex.Message}");
                throw;
            }
        }

        public async Task<Movie> GetMovieById(int id)
        {
            try
            {
                var movie = await _movieRepository.GetMovieAsync(id);

                if (movie is null)
                {
                    throw new Exception("Movie not found");
                }

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetMovieById)}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateMovie(int id, Movie movie)
        {
            try
            {
                var movieToUpdate = await _movieRepository.GetMovieAsync(id);

                if (movieToUpdate is null)
                {
                    throw new Exception("Movie not found");
                }

                movieToUpdate.Title = movie.Title;
                movieToUpdate.Price = movie.Price;
                movieToUpdate.ReleaseDate = movie.ReleaseDate;
                movieToUpdate.Genre = movie.Genre;

                await _movieRepository.UpdateMovieAsync(movieToUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(UpdateMovie)}: {ex.Message}");
                throw;
            }
        }
    }
}
