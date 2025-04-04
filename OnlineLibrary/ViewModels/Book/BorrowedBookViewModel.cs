namespace OnlineLibrary.ViewModels.Book
{
    public class BorrowedBookViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate => BorrowDate.AddDays(14); // 2 weeks borrowing period
        public bool IsOverdue => DueDate < DateTime.UtcNow && ReturnDate == null;
        public DateTime? ReturnDate { get; set; }
    }
}