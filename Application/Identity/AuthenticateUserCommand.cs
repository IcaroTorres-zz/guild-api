using System.Diagnostics.CodeAnalysis;

namespace Application.Identity
{
    [ExcludeFromCodeCoverage]
    public class AuthenticateUserCommand
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
