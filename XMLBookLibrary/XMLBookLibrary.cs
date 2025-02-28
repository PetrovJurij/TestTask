using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace XMLBookLibrary
{
    public class XMLBookLibrary
    {
        private XElement _document;
        private IEnumerable<Book> _books;
        public XMLBookLibrary() { }
        public XMLBookLibrary(string path)
        {
            _document = XElement.Load(path);
        }
        public XMLBookLibrary(XElement document)
        {
            _document = document;
        }

        public IEnumerable<Book> GetBooks => _books;

        public IEnumerable<Book> LoadBooks()
        {
            _books = from book in _document.Descendants("book")
                     select new Book((string)book.Attribute("author"),
                                     (string)book.Attribute("title"),
                                     (int)book.Attribute("pagenumber"));
            return _books;
        }

        public void AddBook(string authorName, string title, int pageNumber)
        {
            _books.Append(new Book(authorName, title, pageNumber));
        }

        public void SaveToFile()
        {
        }

    }
}
