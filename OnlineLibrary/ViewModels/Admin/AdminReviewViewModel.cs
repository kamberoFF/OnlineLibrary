namespace OnlineLibrary.ViewModels
{
    public class AdminReviewViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ReadDate { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}