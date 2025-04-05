namespace OnlineLibrary.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int PublishedYear { get; set; }
        public bool Available { get; set; }
        public bool AvailableToBorrow { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}