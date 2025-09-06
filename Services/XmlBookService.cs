using BookstoreAPI.Models;
using System.Xml.Serialization;

namespace BookstoreAPI.Services
{
    public class XmlBookService : IXmlBookService
    {
        private readonly string _xmlFilePath = "books.xml"; // Hard-coded path as requested
        private readonly XmlSerializer _serializer;

        public XmlBookService()
        {
            _serializer = new XmlSerializer(typeof(Bookstore));
        }

        // טוען את כל הספרים מהקובץ XML
        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            try
            {
                var bookstore = await LoadBookstoreAsync();
                return bookstore.Books.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                // Basic error handling - in real app would use proper logging
                throw new InvalidOperationException($"Error loading books: {ex.Message}");
            }
        }

        // מוצא ספר לפי ISBN
        public async Task<BookDto?> GetBookByIsbnAsync(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return null; // Basic validation
            }

            var bookstore = await LoadBookstoreAsync();
            var book = bookstore.Books.FirstOrDefault(b => b.Isbn == isbn);
            
            return book != null ? MapToDto(book) : null;
        }

        // יוצר ספר חדש
        public async Task<BookDto> CreateBookAsync(CreateBookDto bookDto)
        {
            // Basic validation
            if (string.IsNullOrEmpty(bookDto.Isbn) || string.IsNullOrEmpty(bookDto.Title))
            {
                throw new ArgumentException("ISBN and Title are required");
            }

            var bookstore = await LoadBookstoreAsync();
            
            // Check if book already exists
            if (bookstore.Books.Any(b => b.Isbn == bookDto.Isbn))
            {
                throw new InvalidOperationException("Book with this ISBN already exists");
            }

            var newBook = MapFromCreateDto(bookDto);
            bookstore.Books.Add(newBook);
            
            await SaveBookstoreAsync(bookstore);
            
            return MapToDto(newBook);
        }

        // מעדכן ספר קיים
        public async Task<BookDto?> UpdateBookAsync(string isbn, UpdateBookDto bookDto)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return null;
            }

            var bookstore = await LoadBookstoreAsync();
            var existingBook = bookstore.Books.FirstOrDefault(b => b.Isbn == isbn);
            
            if (existingBook == null)
            {
                return null;
            }

            // Update the book properties
            existingBook.Title.Value = bookDto.Title;
            existingBook.Title.Language = bookDto.Language;
            existingBook.Authors = bookDto.Authors;
            existingBook.Category = bookDto.Category;
            existingBook.Year = bookDto.Year;
            existingBook.Price = bookDto.Price;

            await SaveBookstoreAsync(bookstore);
            
            return MapToDto(existingBook);
        }

        // מוחק ספר
        public async Task<bool> DeleteBookAsync(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return false;
            }

            var bookstore = await LoadBookstoreAsync();
            var bookToRemove = bookstore.Books.FirstOrDefault(b => b.Isbn == isbn);
            
            if (bookToRemove == null)
            {
                return false;
            }

            bookstore.Books.Remove(bookToRemove);
            await SaveBookstoreAsync(bookstore);
            
            return true;
        }

        // יוצר דוח HTML
        public async Task<string> GenerateHtmlReportAsync()
        {
            var books = await GetAllBooksAsync();
            
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Bookstore Report</title>
    <style>
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #f2f2f2; }}
    </style>
</head>
<body>
    <h1>Bookstore Inventory Report</h1>
    <table>
        <thead>
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Category</th>
                <th>Year</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>";

            foreach (var book in books)
            {
                var authors = string.Join(", ", book.Authors);
                html += $@"
            <tr>
                <td>{book.Title}</td>
                <td>{authors}</td>
                <td>{book.Category}</td>
                <td>{book.Year}</td>
                <td>${book.Price:F2}</td>
            </tr>";
            }

            html += @"
        </tbody>
    </table>
</body>
</html>";

            return html;
        }

        // טוען את הקובץ XML
        private async Task<Bookstore> LoadBookstoreAsync()
        {
            if (!File.Exists(_xmlFilePath))
            {
                // Create empty bookstore if file doesn't exist
                return new Bookstore();
            }

            using var fileStream = new FileStream(_xmlFilePath, FileMode.Open, FileAccess.Read);
            var bookstore = (Bookstore)_serializer.Deserialize(fileStream)!;
            return bookstore ?? new Bookstore();
        }

        // שומר את הקובץ XML
        private async Task SaveBookstoreAsync(Bookstore bookstore)
        {
            using var fileStream = new FileStream(_xmlFilePath, FileMode.Create, FileAccess.Write);
            _serializer.Serialize(fileStream, bookstore);
            await fileStream.FlushAsync(); // Make sure data is written
        }

        // Maps Book to BookDto
        private BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Isbn = book.Isbn,
                Title = book.Title.Value,
                Language = book.Title.Language,
                Authors = book.Authors,
                Category = book.Category,
                Year = book.Year,
                Price = book.Price
            };
        }

        // Maps CreateBookDto to Book
        private Book MapFromCreateDto(CreateBookDto dto)
        {
            return new Book
            {
                Isbn = dto.Isbn,
                Title = new BookTitle
                {
                    Value = dto.Title,
                    Language = dto.Language
                },
                Authors = dto.Authors,
                Category = dto.Category,
                Year = dto.Year,
                Price = dto.Price
            };
        }
    }
}
