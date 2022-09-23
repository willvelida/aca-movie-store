using MovieStore.Web.Models;
using MovieStore.Web.Services.Interfaces;
using Refit;

namespace MovieStore.Web.Services
{
    public class CatalogBackendClient : ICatalogBackendClient
    {
        IHttpClientFactory _httpClientFactory;
        HttpClient _client;

        public CatalogBackendClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("Catalog");
        }

        public async Task CreateMovie(Movie movie)
        {           
            await RestService.For<ICatalogBackendClient>(_client).CreateMovie(movie);
        }

        public async Task DeleteMovie(int movieId)
        {
            await RestService.For<ICatalogBackendClient>(_client).DeleteMovie(movieId);
        }

        public async Task<Movie> GetMovie(int movieId)
        {
            return await RestService.For<ICatalogBackendClient>(_client).GetMovie(movieId);
        }

        public async Task<List<Movie>> GetMovies()
        {
            return await RestService.For<ICatalogBackendClient>(_client).GetMovies();
        }

        public async Task UpdateMovie(int movieId, Movie movie)
        {
            await RestService.For<ICatalogBackendClient>(_client).UpdateMovie(movieId, movie);
        }
    }
}
