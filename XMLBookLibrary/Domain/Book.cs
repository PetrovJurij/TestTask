namespace XMLBookLibrary.Domain
{
    public record class Book
    {
        public Book(string author, string title, int pageNumber)
        {
            Author = author;
            Title = title;
            PageNumber = pageNumber;
        }

        public string Author { get; set; }
        public string Title { get; set; }
        public int PageNumber { get; set; }
    }
}