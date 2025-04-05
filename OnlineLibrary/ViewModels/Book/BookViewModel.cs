using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.ViewModels.Book
{
    public class BookViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Author { get; set; }

        [HiddenInput]
        public string AuthorId { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [Range(1000, 2100, ErrorMessage = "Year must be between 1000 and 2100")]
        public int PublishedYear { get; set; }

        public bool Available { get; set; }

        public bool AvailableToBorrow { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}