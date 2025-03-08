using System.Collections.Generic;
using System.Xml.Serialization;

namespace XMLBookLibrary.Domain
{
    [XmlRoot("booklist")]
    public class BookList
    {
        [XmlElement("book")]
        public List<Book> books;
    }
}
