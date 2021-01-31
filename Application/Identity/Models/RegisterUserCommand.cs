using Business.Commands;

namespace Application.Identity.Models
{
    public class RegisterUserCommand : CreationCommand<User, UserDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
