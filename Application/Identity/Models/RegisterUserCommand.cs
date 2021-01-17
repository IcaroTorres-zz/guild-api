using Domain.Commands;

namespace Application.Identity.Models
{
    public class RegisterUserCommand : CreationCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
