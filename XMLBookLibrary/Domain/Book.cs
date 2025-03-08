using System.Xml.Serialization;

namespace XMLBookLibrary.Domain
{
    [XmlType("book")]
    public record Book([property: XmlElement("author")] string Author,
        [property: XmlElement("title")] string Title,
        [property: XmlElement("pagenumber")] int PageNumber)
    {
        public Book() : this(string.Empty, string.Empty, 0) { }
    }

}