using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Repository.Interfaces;

namespace MovieStore.Catalog.Services.UnitTests.ServiceTests
{
    public class MovieServiceShould
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<ILogger<MovieService>> _loggerMock;

        private MovieService _sut;

        public MovieServiceShould()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _loggerMock = new Mock<ILogger<MovieService>>();

            _sut = new MovieService(_movieRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void CreateMovieSuccessfully()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<Movie>();

            Func<Task> serviceAction = async () => await _sut.CreateMovie(movie);

            await serviceAction.Should().NotThrowAsync<Exception>();
            _movieRepositoryMock.Verify(x => x.AddMovieAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public void ThrowExceptionWhenCreateMovieFails()
        {
            _movieRepositoryMock.Setup(x => x.AddMovieAsync(It.IsAny<Movie>())).ThrowsAsync(new Exception());

            Func<Task> serviceAction = async () => await _sut.CreateMovie(It.IsAny<Movie>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in CreateMovie: Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void DeleteMovieSuccessfully()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<Movie>();

            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ReturnsAsync(movie);

            Func<Task> serviceAction = async () => await _sut.DeleteMovie(It.IsAny<int>());

            serviceAction.Should().NotThrowAsync<Exception>();
            _movieRepositoryMock.Verify(x => x.DeleteMovieAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public void ThrowExceptionWhenDeleteMovieFails()
        {
            Func<Task> serviceAction = async () => await _sut.DeleteMovie(It.IsAny<int>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in DeleteMovie: Movie not found");
        }

        [Fact]
        public void ThrowExceptionWhennMovieNotFoundInDeleteMovie()
        {
            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            Func<Task> serviceAction = async () => await _sut.DeleteMovie(It.IsAny<int>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in DeleteMovie: Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void GetAllMoviesSuccessfully()
        {
            var fixture = new Fixture();
            var movies = fixture.CreateMany<Movie>().ToList();

            _movieRepositoryMock.Setup(x => x.GetMoviesAsync()).ReturnsAsync(movies);

            Func<Task> serviceAction = async () => await _sut.GetAllMovies();

            serviceAction.Should().NotThrowAsync<Exception>();
            _movieRepositoryMock.Verify(x => x.GetMoviesAsync(), Times.Once);
        }

        [Fact]
        public void ThrowExceptionWhenGetAllMoviesFails()
        {
            _movieRepositoryMock.Setup(x => x.GetMoviesAsync()).ThrowsAsync(new Exception());

            Func<Task> serviceAction = async () => await _sut.GetAllMovies();

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in GetAllMovies: Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void GetMovieByIdSuccessfully()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<Movie>();

            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ReturnsAsync(movie);

            Func<Task> serviceAction = async () => await _sut.GetMovieById(It.IsAny<int>());

            serviceAction.Should().NotThrowAsync<Exception>();
            _movieRepositoryMock.Verify(x => x.GetMovieAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ThrowExceptionWhenMovieNotFoundInGetMovieById()
        {
            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            Func<Task> serviceAction = async () => await _sut.GetMovieById(It.IsAny<int>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in GetMovieById: Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void ThrowExceptionWhenGetMovieByIdFails()
        {
            Func<Task> serviceAction = async () => await _sut.GetMovieById(It.IsAny<int>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in GetMovieById: Movie not found");
        }

        [Fact]
        public void UpdateMovieSuccessfully()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<Movie>();
            movie.Id = 1;
            var movieToUpdate = fixture.Create<Movie>();
            movieToUpdate.Id = 1;

            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ReturnsAsync(movieToUpdate);

            Func<Task> serviceAction = async () => await _sut.UpdateMovie(It.IsAny<int>(), movie);

            serviceAction.Should().NotThrowAsync<Exception>();
            _movieRepositoryMock.Verify(x => x.UpdateMovieAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public void ThrowExeptionWhenMovieNotFoundInUpdateMovie()
        {
            _movieRepositoryMock.Setup(x => x.GetMovieAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            Func<Task> serviceAction = async () => await _sut.UpdateMovie(It.IsAny<int>(), It.IsAny<Movie>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in UpdateMovie: Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public void ThrowExceptionWhenUpdateMovieFails()
        {
            Func<Task> serviceAction = async () => await _sut.UpdateMovie(It.IsAny<int>(), It.IsAny<Movie>());

            serviceAction.Should().ThrowAsync<Exception>().WithMessage("Exception thrown in UpdateMovie: Movie not found");
        }
    }
}