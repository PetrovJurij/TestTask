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

        [TestCase("XMLBookLibrary.xml",2)]
        [TestCase("XMLBookLibrary1Element.xml",1)]
        public void XMLBookLibrary_LoadBooks_AmountMatches(string filePath, int expectedAmount)
        {
            //Arrange
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            //Act
            var books = xmlLibrary.LoadBooks();

            //Assert
            Assert.That(books, Is.Not.Empty);
            Assert.That(expectedAmount, Is.EqualTo(books.Count()));
        }

        [TestCase("XMLBookLibrary_ParsingException.xml")]
        public void XMLBookLibrary_LoadBooks_Empty(string filePath)
        {
            //Arrange
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            //Act
            var books = xmlLibrary.LoadBooks();

            //Assert
            Assert.IsEmpty(books);
        }

        [TestCase("XMLBookLibrary_ExpectedStructure.xml","XMLBook.xml")]
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

        [TestCase("Test","Test",1)]
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
    }
}