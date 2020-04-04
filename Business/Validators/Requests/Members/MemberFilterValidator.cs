using Business.Commands.Members;
using FluentValidation;

namespace Business.Validators.Requests.Members
{
	public class MemberFilterValidator : AbstractValidator<MemberFilterCommand>
	{
		public MemberFilterValidator()
		{
			RuleFor(x => x.Count).GreaterThan(0);
		}
	}
}