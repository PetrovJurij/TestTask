using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMLBookLibrary.Domain;

namespace XMLBookLibrary
{
    public class XMLBookLibrary
    {
        private XElement _document;
        private string _path;
        private List<Book> _books = new List<Book>();

        public XMLBookLibrary(string path)
        {
            _path = path;
            _document = XElement.Load(_path);
            LoadBooks();
        }
        public XMLBookLibrary(XDocument document)
        {
            _path = document.BaseUri;
            _document = document.Root;
            LoadBooks();
        }

        public IEnumerable<Book> Books => _books;

        public IEnumerable<Book> LoadBooks()
        {
            var authorElementName = XMLStructureBuilder.GetMemberName((Book b) => b.Author).ToLower();
            var titleElementName = XMLStructureBuilder.GetMemberName((Book b) => b.Title).ToLower();
            var pageNumberElementName = XMLStructureBuilder.GetMemberName((Book b) => b.PageNumber).ToLower();

            var books = from book in _document.Descendants(nameof(Book).ToLower())
                        where book.Elements(authorElementName).Any() 
                        && book.Elements(titleElementName).Any()
                        && book.Elements(pageNumberElementName).Any()
                        select new Book((string)book.Element(authorElementName),
                                        (string)book.Element(titleElementName),
                                        (int)book.Element(pageNumberElementName));
            _books = books.ToList();
            return _books;
        }

        public void AddBook(string authorName, string title, int pageNumber)
        {
             _books.Add(new Book(authorName, title, pageNumber));
        }

        public IEnumerable<Book> SortBooksByAuthorThenByTitle()
        {
            var sortedBooks = from book in _books
                              orderby book.Author, book.Title
                              select book;
            _books = [.. sortedBooks];
            return _books;
        }

        public IEnumerable<Book> SearchBooksByTitle(string title)
        {
            var searchBooks = from book in _books
                              where book.Title.Contains(title, 
                                      System.StringComparison.InvariantCultureIgnoreCase)
                              select book;
            return searchBooks;
        }


        public void Save()
        {
            _document = XMLStructureBuilder.CreateXMLStructure(_books);
            var xmlDocument = new XDocument(_document);
            xmlDocument.Save(_path);
        }
        public void SaveToFile(string path)
        {
            var document = XMLStructureBuilder.CreateXMLStructure(_books);
            var xmlDocument = new XDocument(document);
            xmlDocument.Save(path);
        }

    }
}
