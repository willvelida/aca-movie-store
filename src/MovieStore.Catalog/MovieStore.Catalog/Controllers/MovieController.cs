using Microsoft.AspNetCore.Mvc;
using MovieStore.Catalog.Common.Models;
using MovieStore.Catalog.Services.Interfaces;

namespace MovieStore.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger _logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        // GET: api/Movie
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                var movies = await _movieService.GetAllMovies();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetMovies)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieById(id);

                if (movie is null)
                {
                    _logger.LogError($"Movie for ID {id} does not exist");
                    return NotFound();
                }

                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetMovie)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            try
            {
                if (id != movie.ID)
                {
                    _logger.LogError($"Movie ID mismatch.");
                    return BadRequest();
                }

                await _movieService.UpdateMovie(id, movie);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(UpdateMovie)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Movie
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            try
            {
                await _movieService.CreateMovie(movie);

                return CreatedAtAction("GetMovie", new { id = movie.ID }, movie);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(CreateMovie)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieService.DeleteMovie(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(DeleteMovie)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
