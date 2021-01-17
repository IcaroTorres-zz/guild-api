using FluentValidation;

namespace Business.Usecases.Guilds.ListGuild
{
    public class ListGuildValidator : AbstractValidator<ListGuildCommand>
    {
        public ListGuildValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
        }
    }
}