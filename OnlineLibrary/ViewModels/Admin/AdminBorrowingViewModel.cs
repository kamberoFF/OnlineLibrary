namespace OnlineLibrary.ViewModels
{
    public class AdminBorrowingViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsOverdue => DueDate < DateTime.UtcNow && ReturnDate == null;
        public DateTime? ReturnDate { get; set; }
    }
}