using Microsoft.AspNetCore.Identity;

namespace Assignment2.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string ContactInfo { get; set; }
        public string PreferredCategories { get; set; } // Optional: Comma-separated list of categories
    }
}