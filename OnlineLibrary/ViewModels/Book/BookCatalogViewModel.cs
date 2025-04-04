namespace OnlineLibrary.ViewModels.Book
{
    public class BookCatalogViewModel
    {
        public List<BookViewModel> Books { get; set; }
        public string SearchString { get; set; }
        public string Genre { get; set; }
        public List<string> Genres { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}