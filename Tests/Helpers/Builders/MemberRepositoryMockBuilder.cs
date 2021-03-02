using Application.Common.Abstractions;
using Application.Members.Queries.ListMember;
using Domain.Models;
using Moq;
using System;
using System.Linq.Expressions;
using Tests.Domain.Models.Fakes;

namespace Tests.Helpers.Builders
{
    public sealed class MemberRepositoryMockBuilder
    {
        private readonly Mock<IMemberRepository> _mock;

        private MemberRepositoryMockBuilder()
        {
            _mock = new Mock<IMemberRepository>();
        }

        public static MemberRepositoryMockBuilder Create()
        {
            return new MemberRepositoryMockBuilder();
        }

        public MemberRepositoryMockBuilder ExistsWithId(bool result, Guid? id = null)
        {
            if (id == null)
            {
                _mock.Setup(x => x.ExistsWithIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.ExistsWithIdAsync(id.Value, default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder ExistsWithName(bool result, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _mock.Setup(x => x.ExistsWithNameAsync(It.IsAny<string>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.ExistsWithNameAsync(name, default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder GetByIdSuccess(Guid? input = null, Member output = null)
        {
            var result = output ?? MemberFake.GuildLeader().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetByIdAsync(input.Value, It.IsAny<bool>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder GetByIdFail(Guid? id = null)
        {
            var result = Member.Null;

            if (id == null)
            {
                _mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetByIdAsync(id.Value, It.IsAny<bool>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder GetForGuildOperationsSuccess(Guid? input = null, Member output = null)
        {
            var result = output ?? MemberFake.GuildLeader().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.GetForGuildOperationsAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetForGuildOperationsAsync(input.Value, default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder GetForGuildOperationsFail(Guid? id = null)
        {
            var result = Member.Null;

            if (id == null)
            {
                _mock.Setup(x => x.GetForGuildOperationsAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetForGuildOperationsAsync(id.Value, default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder CanChangeName(bool result, Guid id, string name)
        {
            _mock.Setup(x => x.CanChangeNameAsync(id, name, default)).ReturnsAsync(result);
            return this;
        }

        public MemberRepositoryMockBuilder IsGuildMember(bool result, Guid id, Guid guildId)
        {
            _mock.Setup(x => x.IsGuildMemberAsync(id, guildId, default)).ReturnsAsync(result);
            return this;
        }

        public MemberRepositoryMockBuilder Insert(string name, Member output = null)
        {
            var result = output ?? MemberFake.WithoutGuild().Generate();
            _mock.Setup(x => x.InsertAsync(It.Is<Member>(x => x.Name == name), default)).ReturnsAsync(result);
            return this;
        }

        public MemberRepositoryMockBuilder Update(Member input = null, Member output = null)
        {
            var result = output ?? MemberFake.GuildLeader().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.Update(It.IsAny<Member>())).Returns(result);
            }
            else
            {
                _mock.Setup(x => x.Update(input)).Returns(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder Paginate(ListMemberCommand command, int totalItems)
        {
            var PagedResponse = PagedResponseFake.PaginateMembers(command, totalItems).Generate();

            _mock.Setup(x => x.PaginateAsync(It.IsAny<Expression<Func<Member, bool>>>(), command.PageSize, command.Page, default))
                 .ReturnsAsync(PagedResponse);

            return this;
        }

        public IMemberRepository Build()
        {
            return _mock.Object;
        }
    }
}
