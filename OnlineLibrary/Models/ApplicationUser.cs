using Microsoft.AspNetCore.Identity;

namespace OnlineLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Change the role for production
        public string Role { get; set; } = "Admin";
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}