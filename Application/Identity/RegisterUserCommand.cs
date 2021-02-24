using Application.Common.Commands;
using Application.Common.Responses;

namespace Application.Identity
{
    public class RegisterUserCommand : CreationCommand<User, UserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
