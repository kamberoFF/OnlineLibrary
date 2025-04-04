namespace OnlineLibrary.ViewModels.Book
{
    public class ReadingHistoryViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public DateTime ReadDate { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}