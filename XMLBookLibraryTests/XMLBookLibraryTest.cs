using System.Xml.Linq;
using XMLBookLibrary.Domain;

namespace XMLBookLibraryTests
{
    [TestFixture]
    public class XMLBookLibraryTest
    {
        public string _filePath;
        public string _defaultFile = "XMLBookLibrary.xml";

        [SetUp]
        public void Setup()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(baseDirectory, "TestCaseFiles");
        }

        [TestCase("XMLBookLibrary.xml", 3)]
        [TestCase("XMLBookLibrary1Element.xml", 1)]
        public void XMLBookLibrary_DocumentConstructor_AmountMatches(string filePath, int expectedAmount)
        {
            //Arrange
            var path = Path.Combine(_filePath, filePath);
            var document = XDocument.Load(path);
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(document);

            //Act
            var books = xmlLibrary.LoadBooks();

            //Assert
            Assert.That(books, Is.Not.Empty);
            Assert.That(expectedAmount, Is.EqualTo(books.Count()));
        }

        [TestCase("XMLBookLibrary.xml", 3)]
        [TestCase("XMLBookLibrary1Element.xml", 1)]
        [TestCase("XMLBookLibrary_NoBooks.xml", 0)]
        [TestCase("XMLBookLibraryDuplicateBooks.xml", 4)]
        public void XMLBookLibrary_LoadBooks_AmountMatches(string filePath, int expectedAmount)
        {
            //Arrange
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            //Act
            var books = xmlLibrary.LoadBooks();

            //Assert
            Assert.That(expectedAmount, Is.EqualTo(books.Count()));
        }

        [TestCase("XMLBookLibrary.xml", 3)]
        [TestCase("XMLBookLibrary1Element.xml", 1)]
        [TestCase("XMLBookLibraryDuplicateBooks.xml", 4)]
        public void XMLBookLibrary_LoadBooksXMLSerializer_AmountMatches(string filePath, int expectedAmount)
        {
            //Arrange
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            //Act
            var books = xmlLibrary.LoadBooksXMLSerializer();

            //Assert
            Assert.That(books.Count(), Is.EqualTo(expectedAmount));
        }

        [TestCase("XMLBookLibrary_NoBooks.xml", 0)]
        public void XMLBookLibrary_LoadBooksXMLSerializer_ThrowsInvalidOperationException(string filePath, int expectedAmount)
        {
            //Arrange
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            //Act && Assert
            Assert.That(() => xmlLibrary.LoadBooksXMLSerializer(), Throws.InvalidOperationException);
        }

        [TestCase("XMLBookLibrary_Save_ExpectedStructure.xml", "XMLBook.xml")]
        public void XMLBookLibrary_SaveToFile_AllSaved(string expectedFileName, string fileName)
        {
            //Arrange
            var initialPath = Path.Combine(_filePath, _defaultFile);
            var tastPath = Path.Combine(_filePath, fileName);
            var initialXmlLibrary = new XMLBookLibrary.XMLBookLibrary(initialPath);

            var expectedPath = Path.Combine(_filePath, expectedFileName);
            var expectedXmlLibrary = new XMLBookLibrary.XMLBookLibrary(expectedPath);

            //Act
            initialXmlLibrary.AddBook("AuthorTest", "TitleTest1", 9);
            initialXmlLibrary.SaveToFile(tastPath);

            var testBooks = new XMLBookLibrary.XMLBookLibrary(tastPath).Books;
            var expectedBooks = expectedXmlLibrary.Books;

            //Assert
            CollectionAssert.AreEqual(expectedBooks, testBooks);
        }

        [TestCase("XMLBookLibrary_Save.xml")]
        public void XMLBookLibrary_Save_AllSaved(string fileName)
        {
            //Arrange
            var expectedAmountOfBooks = 4;
            var initialPath = Path.Combine(_filePath, fileName);
            var initialXmlLibrary = new XMLBookLibrary.XMLBookLibrary(initialPath);

            //Act
            initialXmlLibrary.AddBook("AuthorTest", "TitleTest3", 10);
            initialXmlLibrary.Save();
            var amountOfBooks = initialXmlLibrary.LoadBooks();

            //Assert
            Assert.That(amountOfBooks.Count(), Is.EqualTo(expectedAmountOfBooks));
        }

        [TestCase("XMLBookLibrary_SaveWithXMLSerializerToFile_ExpectedStructure.xml", "XMLBook_1.xml")]
        public void XMLBookLibrary_SaveWithXMLSerializerToFile_AllSaved(string expectedFileName, string fileName)
        {
            //Arrange
            var initialPath = Path.Combine(_filePath, _defaultFile);
            var tastPath = Path.Combine(_filePath, fileName);
            var initialXmlLibrary = new XMLBookLibrary.XMLBookLibrary(initialPath);

            var expectedPath = Path.Combine(_filePath, expectedFileName);
            var expectedXmlLibrary = new XMLBookLibrary.XMLBookLibrary(expectedPath);

            //Act
            initialXmlLibrary.AddBook("AuthorTest", "TitleTest1", 9);
            initialXmlLibrary.SaveWithXMLSerializerToFile(tastPath);

            var testBooks = new XMLBookLibrary.XMLBookLibrary(tastPath).Books;
            var expectedBooks = expectedXmlLibrary.Books;

            //Assert
            CollectionAssert.AreEqual(expectedBooks, testBooks);
        }

        [TestCase("XMLBookLibrary_SaveWithXMLSerializer.xml")]
        public void XMLBookLibrary_SaveWithXMLSerializer_AllSaved(string fileName)
        {
            //Arrange
            var expectedAmountOfBooks = 4;
            var initialPath = Path.Combine(_filePath, fileName);
            var initialXmlLibrary = new XMLBookLibrary.XMLBookLibrary(initialPath);

            //Act
            initialXmlLibrary.AddBook("AuthorTest", "TitleTest3", 10);
            initialXmlLibrary.SaveWithXMLSerializer();
            var amountOfBooks = initialXmlLibrary.LoadBooks();

            //Assert
            Assert.That(amountOfBooks.Count(), Is.EqualTo(expectedAmountOfBooks));
        }

        [TestCase("Test", "Test", 1)]
        public void XMLBookLibrary_AddBook_Saved(string author, string title, int pageNumber)
        {
            //Arrange
            var expectedBook = new Book(author, title, pageNumber);
            var path = Path.Combine(_filePath, _defaultFile);
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(path);

            //Act
            xmlLibrary.AddBook(author, title, pageNumber);
            var books = xmlLibrary.Books;

            //Assert
            CollectionAssert.Contains(books, expectedBook);
        }


        [TestCase("1",1)]
        [TestCase("Test", 2)]
        public void XMLBookLibrary_Search(string searchCriteria,int expectedAmount)
        {
            //Arrange
            var path = Path.Combine(_filePath, _defaultFile);
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(path);

            //Act
            var books = xmlLibrary.SearchBooksByTitle(searchCriteria);

            //Assert
            Assert.That(books.Count(), Is.EqualTo(expectedAmount));
        }

        [TestCase("XMLBookLibrary_Sort.xml")]
        public void XMLBookLibrary_Sort_Sorted(string fileName)
        {
            //Arrange
            var path = Path.Combine(_filePath, fileName);
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(path);

            //Act
            var books = xmlLibrary.SortBooksByAuthorThenByTitle();

            //Assert
            CollectionAssert.IsOrdered(books, new BookComparer());

        }

        public class BookComparer : Comparer<Book>
        {
            public override int Compare(Book? x, Book? y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }
                if (x.Author.CompareTo(y?.Author) != 0)
                {
                    return 0;
                }

                return x.Title.CompareTo(y?.Title);
            }
        }
    }
}