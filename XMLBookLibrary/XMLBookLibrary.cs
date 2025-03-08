using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using XMLBookLibrary.Domain;

namespace XMLBookLibrary
{
    public class XMLBookLibrary
    {
        private XElement _document;
        private string _path;
        private BookList _books = new BookList{books = [] };
        private XmlSerializer _serializer = new(typeof(BookList));

        public XMLBookLibrary(string path)
        {
            _path = path;
            _document = XElement.Load(_path);
            SetHandlersForUnexpectedattributes();
            LoadBooks();
        }


        public XMLBookLibrary(XDocument document)
        {
            _path = document.BaseUri;
            _document = document.Root;
            SetHandlersForUnexpectedattributes();
            LoadBooks();
        }
        private void SetHandlersForUnexpectedattributes()
        {
            _serializer.UnknownNode += new
            XmlNodeEventHandler(Serializer_UnknownNode);
            _serializer.UnknownAttribute += new
            XmlAttributeEventHandler(Serializer_UnknownAttribute);
        }

        public IEnumerable<Book> Books => _books.books;

        public IEnumerable<Book> LoadBooks()
        {
            var authorElementName = XMLStructureMapper.GetMemberName((Book b) => b.Author).ToLower();
            var titleElementName = XMLStructureMapper.GetMemberName((Book b) => b.Title).ToLower();
            var pageNumberElementName = XMLStructureMapper.GetMemberName((Book b) => b.PageNumber).ToLower();

            var books = from book in _document.Descendants(nameof(Book).ToLower())
                        where book.Elements(authorElementName).Any()
                        && book.Elements(titleElementName).Any()
                        && book.Elements(pageNumberElementName).Any()
                        select new Book((string)book.Element(authorElementName),
                                        (string)book.Element(titleElementName),
                                        (int)book.Element(pageNumberElementName));
            _books.books = [.. books?.Distinct()];
            return _books.books;
        }

        public IEnumerable<Book> LoadBooksXMLSerializer()
        {
            using var stream = new FileStream(_path, FileMode.Open);
            var bookList = _serializer.Deserialize(stream) as BookList;

            if (bookList.books?.Distinct() == null)
            {
                _books.books = [];
                return _books.books;
            }

            var books = (from book in bookList.books.Distinct()
                         where !String.IsNullOrEmpty(book.Author)
                                && !String.IsNullOrEmpty(book.Author)
                                && book.PageNumber != 0
                         select book).ToList();
            _books.books = [.. books?.Distinct()];

            return _books.books;
        }


        public void AddBook(string authorName, string title, int pageNumber)
        {
            _books.books.Add(new Book(authorName, title, pageNumber));
        }

        public IEnumerable<Book> SortBooksByAuthorThenByTitle()
        {
            var sortedBooks = from book in _books.books
                              orderby book.Author, book.Title
                              select book;
            _books.books = [.. sortedBooks];
            return _books.books;
        }

        public IEnumerable<Book> SearchBooksByTitle(string title)
        {
            var searchBooks = from book in _books.books
                              where book.Title.Contains(title,
                                      System.StringComparison.InvariantCultureIgnoreCase)
                              select book;
            return searchBooks;
        }

        public void Save()
        {
            _document = XMLStructureMapper.CreateXMLStructure(_books.books);
            var xmlDocument = new XDocument(_document);
            xmlDocument.Save(_path);
        }
        public void SaveToFile(string path)
        {
            var document = XMLStructureMapper.CreateXMLStructure(_books.books);
            var xmlDocument = new XDocument(document);
            xmlDocument.Save(path);
        }
        public void SaveWithXMLSerializer()
        {
            using (var stream = new FileStream(_path, FileMode.OpenOrCreate))
            {
                _serializer.Serialize(stream, _books);
            }

            _document = XElement.Load(_path);
        }        
        public void SaveWithXMLSerializerToFile(string path)
        {
            using var stream = new FileStream(path, FileMode.OpenOrCreate);
            _serializer.Serialize(stream, _books);
        }
        private void Serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void Serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
