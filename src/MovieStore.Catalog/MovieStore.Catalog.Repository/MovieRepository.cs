using Microsoft.EntityFrameworkCore;
using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Catalog.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;

        public MovieRepository(MovieContext context)
        {
            _context = context;
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
                throw;
            }
        }
    }
}
