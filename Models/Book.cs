using System.Xml.Serialization;

namespace BookstoreAPI.Models
{
    [XmlRoot("book")]
    public class Book
    {
        [XmlAttribute("category")]
        public string Category { get; set; } = string.Empty;

        [XmlElement("isbn")]
        public string Isbn { get; set; } = string.Empty;

        [XmlElement("title")]
        public BookTitle Title { get; set; } = new BookTitle();

        [XmlElement("author")]
        public List<string> Authors { get; set; } = new List<string>();

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }

    public class BookTitle
    {
        [XmlAttribute("lang")]
        public string Language { get; set; } = "en";

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }
}
