using API.Models.Roles;
using Microsoft.AspNetCore.Identity;
using MyBlog.BLL.Models;

namespace API.Models.Users
{
    public class GetUsersResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email {get; set; }
        public DateTime BirthDate { get; set; }
        public List<RoleViews> Roles { get; set; } = new List<RoleViews>();
    }
}
