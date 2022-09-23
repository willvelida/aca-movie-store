using MovieStore.Web.Models;
using Refit;

namespace MovieStore.Web.Services.Interfaces
{
    public interface ICatalogBackendClient
    {
        [Get("api/Movie")]
        Task<List<Movie>> GetMovies();

        [Get("api/Movie/{movieId}")]
        Task<Movie> GetMovie(int movieId);

        [Put("api/Movie/{movieId}")]
        Task UpdateMovie(int movieId, Movie movie);

        [Post("api/Movie")]
        Task CreateMovie(Movie movie);

        [Delete("api/Movie/{movieId}")]
        Task DeleteMovie(int movieId);
    }
}
