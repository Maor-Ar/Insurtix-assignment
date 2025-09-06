using BookstoreAPI.Models;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IXmlBookService _bookService;

        public BooksController(IXmlBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Basic error handling - returns 500
                return StatusCode(500, new { message = "Error retrieving books", error = ex.Message });
            }
        }

        // GET: api/books/{isbn}
        [HttpGet("{isbn}")]
        public async Task<ActionResult<BookDto>> GetBook(string isbn)
        {
            // Basic validation
            if (string.IsNullOrEmpty(isbn))
            {
                return BadRequest(new { message = "ISBN is required" });
            }

            try
            {
                var book = await _bookService.GetBookByIsbnAsync(isbn);
                
                if (book == null)
                {
                    return NotFound(new { message = "Book not found" });
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving book", error = ex.Message });
            }
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto bookDto)
        {
            // Basic validation
            if (bookDto == null)
            {
                return BadRequest(new { message = "Book data is required" });
            }

            if (string.IsNullOrEmpty(bookDto.Isbn) || string.IsNullOrEmpty(bookDto.Title))
            {
                return BadRequest(new { message = "ISBN and Title are required" });
            }

            // Basic ISBN validation (just check length)
            if (bookDto.Isbn.Length < 10)
            {
                return BadRequest(new { message = "Invalid ISBN format" });
            }

            try
            {
                var createdBook = await _bookService.CreateBookAsync(bookDto);
                return CreatedAtAction(nameof(GetBook), new { isbn = createdBook.Isbn }, createdBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating book", error = ex.Message });
            }
        }

        // PUT: api/books/{isbn}
        [HttpPut("{isbn}")]
        public async Task<ActionResult<BookDto>> UpdateBook(string isbn, [FromBody] UpdateBookDto bookDto)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return BadRequest(new { message = "ISBN is required" });
            }

            if (bookDto == null)
            {
                return BadRequest(new { message = "Book data is required" });
            }

            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(isbn, bookDto);
                
                if (updatedBook == null)
                {
                    return NotFound(new { message = "Book not found" });
                }

                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating book", error = ex.Message });
            }
        }

        // DELETE: api/books/{isbn}
        [HttpDelete("{isbn}")]
        public async Task<ActionResult> DeleteBook(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return BadRequest(new { message = "ISBN is required" });
            }

            try
            {
                var deleted = await _bookService.DeleteBookAsync(isbn);
                
                if (!deleted)
                {
                    return NotFound(new { message = "Book not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting book", error = ex.Message });
            }
        }

        // GET: api/books/report
        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            try
            {
                var htmlReport = await _bookService.GenerateHtmlReportAsync();
                return Content(htmlReport, "text/html");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating report", error = ex.Message });
            }
        }
    }
}
