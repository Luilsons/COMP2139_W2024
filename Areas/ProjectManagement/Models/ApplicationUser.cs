using Microsoft.AspNetCore.Identity;
namespace MVC_Application.Areas.ProjectManagement.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UsernaameChageLimit { get; set; } = 10;

        public byte[]? ProfilePicture { get; set; }
        public int UsernameChangeLimit { get; internal set; }
    }
}
