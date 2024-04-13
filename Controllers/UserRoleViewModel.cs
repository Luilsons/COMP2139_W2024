
namespace MVC_Application.Controllers
{
    internal class UserRoleViewModel
    {
        public string UserId { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string? Email { get; internal set; }
        public List<string> Roles { get; internal set; }
    }
}