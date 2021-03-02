using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Models;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Queries.ListMember
{
    public class ListMemberHandler : IRequestHandler<ListMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public ListMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(ListMemberCommand command, CancellationToken cancellationToken)
        {
            Expression<Func<Member, bool>> memberfilter =
                e => (command.GuildId == null || e.GuildId == command.GuildId) &&
                     (string.IsNullOrWhiteSpace(command.Name) || e.Name.Contains(command.Name, StringComparison.OrdinalIgnoreCase));

            var pagedMembers = await _memberRepository.PaginateAsync(
                predicate: memberfilter,
                top: command.PageSize,
                page: command.Page,
                cancellationToken: cancellationToken);

            pagedMembers.SetAppliedCommand(command);
            return new SuccessResult(pagedMembers);
        }
    }
}