using MovieStore.Catalog.Common.Models;

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
