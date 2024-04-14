using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QLBangHangA.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int Age;
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }

        public DateTime? Birthday { get; internal set; }
        public string? ProfilePicture { get; internal set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
