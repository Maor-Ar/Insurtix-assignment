namespace BookstoreAPI.Models
{
    // DTO for API requests and responses
    public class BookDto
    {
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "en";
        public List<string> Authors { get; set; } = new List<string>();
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
    }

    // DTO for creating new books
    public class CreateBookDto
    {
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "en";
        public List<string> Authors { get; set; } = new List<string>();
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
    }

    // DTO for updating books
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "en";
        public List<string> Authors { get; set; } = new List<string>();
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}
