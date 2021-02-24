using FluentValidation;

namespace Application.Guilds.Queries.ListGuild
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