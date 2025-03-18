using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime RegistrationTime { get; set; }

        public bool IsActive { get; set; }
    }
}