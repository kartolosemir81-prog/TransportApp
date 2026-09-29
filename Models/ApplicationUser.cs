using Microsoft.AspNetCore.Identity;

namespace TransportApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
