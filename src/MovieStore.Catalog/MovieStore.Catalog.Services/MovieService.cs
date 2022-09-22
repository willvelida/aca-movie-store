using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Repository.Interfaces;
using MovieStore.Catalog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Catalog.Services
{
    public class MovieService : IMovieService
    {
        private IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository=movieRepository;
        }

        public async Task CreateMovie(Movie movie)
        {
            try
            {
                await _movieRepository.AddMovieAsync(movie);
            }
            catch (Exception ex)
            {
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
            catch (Exception)
            {
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
                throw;
            }
        }
    }
}
