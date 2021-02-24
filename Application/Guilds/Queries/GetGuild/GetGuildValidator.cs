using FluentValidation;

namespace Application.Guilds.Queries.GetGuild
{
    public class GetGuildValidator : AbstractValidator<GetGuildCommand>
    {
        public GetGuildValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}