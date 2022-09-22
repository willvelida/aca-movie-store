using MovieStore.Catalog.Common.Models;

namespace MovieStore.Catalog.Repository.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieAsync(int id);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Movie movie);
    }
}
