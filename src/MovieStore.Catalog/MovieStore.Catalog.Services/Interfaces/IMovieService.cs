using MovieStore.Catalog.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Catalog.Services.Interfaces
{
    public interface IMovieService
    {
        Task CreateMovie(Movie movie);
        Task DeleteMovie(int id);
        Task<List<Movie>> GetAllMovies();
        Task<Movie> GetMovieById(int id);
        Task UpdateMovie(int id, Movie movie);
    }
}
