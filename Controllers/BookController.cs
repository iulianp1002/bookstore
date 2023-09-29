using BookstoreAPI.DomainModels;
using BookstoreAPI.Service;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        const string FILE_PATH = @"C:\Samples\";
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>
        /// Get the list of all books
        /// </summary>
        /// <returns></returns>
        [HttpGet("BookList")]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.BookListAsync();
                
                return Ok(books);
            }
            catch (Exception ex)
            {
               _logger.LogError("GetAllBook - "+ ex.Message, ex);
               return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Get the list of all books with their authors
        /// </summary>
        /// <returns></returns>
        [HttpGet("BookListWithAuthors")]
        public async Task<ActionResult<List<BookWithAuthorsModel>>> GetAllBooksWithAuthors()
        {
            try
            {
                var booksWithAuthors = await _bookService.BookListWithAuthorsAsync();

                return Ok(booksWithAuthors);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllBookWithAuthors - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.FindByIdAsync(id);

                if (book == null)
                {
                    return BadRequest("Book not exist!");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetBook - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBook([FromBody]AddBookModel model)
        {
            try
            {
                var result = await _bookService.AddBookAsync(model);
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("CreateBook - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateBook([FromBody]AddBookModel bookModel)
        {
            try
            {
                var book = await _bookService.FindByIdAsync(bookModel.Id);
                if (book == null) return NotFound();

                var result = await _bookService.UpdateBookAsync(bookModel);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("UpdateAuthor - " + ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("id")]
        public async Task<ActionResult<int>> DeleteBook(int id)
        {
            try
            {
                var result = await _bookService.DeleteAsync(id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("DeleteAuthor - " + ex.Message, ex);
                return NotFound(ex.Message);
            }
            

        }

        [HttpPost("Upload")]
        public IActionResult Post([FromBody] FileUploadModel theFile)
        {
            // Create unique file name
            var filePathName = FILE_PATH +
                Path.GetFileNameWithoutExtension(theFile.FileName) + "-" +
                DateTime.Now.ToString().Replace("/", "")
                .Replace(":", "").Replace(" ", "") +
                Path.GetExtension(theFile.FileName);

            // Remove file type from base64 encoding, if any
            if (theFile.FileAsBase64.Contains(","))
            {
                theFile.FileAsBase64 = theFile.FileAsBase64
                  .Substring(theFile.FileAsBase64.IndexOf(",") + 1);
            }

            // Convert base64 encoded string to binary
            theFile.FileAsByteArray = Convert.FromBase64String(theFile.FileAsBase64);

            // Write binary file to server path
            using (var fs = new FileStream(filePathName, FileMode.CreateNew))
            {
                fs.Write(theFile.FileAsByteArray, 0, theFile.FileAsByteArray.Length);
            }
            return Ok();
        }
    }
}
