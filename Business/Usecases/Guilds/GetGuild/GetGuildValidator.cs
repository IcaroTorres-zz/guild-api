using FluentValidation;

namespace Business.Usecases.Guilds.GetGuild
{
    public class GetGuildValidator : AbstractValidator<GetGuildCommand>
    {
        public GetGuildValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}