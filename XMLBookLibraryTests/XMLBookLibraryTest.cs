namespace XMLBookLibraryTests
{
    public class XMLBookLibraryTest
    {
        public string _filePath;

        [SetUp]
        public void Setup()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(baseDirectory, "TestCaseFiles");
        }

        [TestCase("XMLBookLibrary.xml")]
        public void XMLBookLibrary_FileLoads(string filePath)
        {
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));
            
            Assert.Pass();
        }

        [TestCase("XMLBookLibrary_ParsingException.xml")]
        public void XMLBookLibrary_GetBooksFromFile_ParsingException(string filePath)
        {
            var xmlLibrary = new XMLBookLibrary.XMLBookLibrary(Path.Combine(_filePath, filePath));

            Assert.Throws<Exception>(() => xmlLibrary.LoadBooks());
        }
    }
}