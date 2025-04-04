namespace OnlineLibrary.Models
{
    public class ReadingHistory
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public DateTime ReadDate { get; set; } = DateTime.UtcNow;
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}