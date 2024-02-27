using API.Models.Roles;

namespace API.Models.Users
{
    public class EditUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
