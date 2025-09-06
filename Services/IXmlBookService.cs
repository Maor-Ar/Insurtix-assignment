using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IXmlBookService
    {
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIsbnAsync(string isbn);
        Task<BookDto> CreateBookAsync(CreateBookDto bookDto);
        Task<BookDto?> UpdateBookAsync(string isbn, UpdateBookDto bookDto);
        Task<bool> DeleteBookAsync(string isbn);
        Task<string> GenerateHtmlReportAsync();
    }
}
