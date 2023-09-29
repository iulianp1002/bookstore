using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Service;
using BookstoreAPI.DomainModels;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }
        /// <summary>
        /// Get the list of all authors
        /// </summary>
        /// <returns></returns>
        [HttpGet("AuthorsList")]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            try
            {
                var authors = await _authorService.AuthorListAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllAuthors - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
           
        }
        /// <summary>
        /// Get an author
        /// </summary>
        /// <param name="id">id of the author</param>
        /// <returns>the author</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {
                var author = await _authorService.FindByIdAsync(id);

                if (author == null)
                {
                    return BadRequest("Author not exist!");
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAuthor - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// Create an Author
        /// </summary>
        /// <param name="author">author </param>
        /// <returns>returns the id of the created author</returns>
        [HttpPost]
        public  async Task<ActionResult<int>> CreateAuthor(Author author)
        {
            try
            {
                var result = await _authorService.AddAuthorAsync(author);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("CreateAuthor - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Update the author
        /// </summary>
        /// <param name="author"></param>
        /// <returns>result of the updated operation</returns>
        [HttpPut]
        public async Task<ActionResult<int>> UpdateAuthor(Author author)
        {
            try
            {
                var result = await _authorService.UpdateAuthorAsync(author);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("UpdateAuthor - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
           
        }
        /// <summary>
        /// Delete the author
        /// </summary>
        /// <param name="id">id of an author</param>
        /// <returns>result of the delete operation as an integer</returns>
        [HttpDelete("id")]
        public async Task<ActionResult<int>> DeleteAuthor(int id)
        {
            try
            {
                var result = await _authorService.DeleteAsync(id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("DeleteAuthor - " + ex.Message, ex);
                return NotFound(ex.Message);
            }
            

        }

    }
}
