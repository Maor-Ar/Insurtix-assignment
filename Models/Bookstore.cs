using System.Xml.Serialization;

namespace BookstoreAPI.Models
{
    [XmlRoot("bookstore")]
    public class Bookstore
    {
        [XmlElement("book")]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
