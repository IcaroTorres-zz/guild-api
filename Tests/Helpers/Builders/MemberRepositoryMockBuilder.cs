using Application.Common.Abstractions;
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

        public MemberRepositoryMockBuilder CanChangeName(bool result, Guid? id = null, string name = null)
        {
            if (id == null && name == null)
            {
                _mock.Setup(x => x.CanChangeNameAsync(It.IsAny<Guid>(), It.IsAny<string>(), default)).ReturnsAsync(result);
            }
            else if (id == null)
            {
                _mock.Setup(x => x.CanChangeNameAsync(It.IsAny<Guid>(), name, default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.CanChangeNameAsync(id.Value, It.IsAny<string>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder IsGuildMember(bool result, Guid? id = null, Guid? guildId = null)
        {
            if (id == null && guildId == null)
            {
                _mock.Setup(x => x.IsGuildMemberAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else if (id == null)
            {
                _mock.Setup(x => x.IsGuildMemberAsync(It.IsAny<Guid>(), guildId.Value, default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.IsGuildMemberAsync(id.Value, It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public MemberRepositoryMockBuilder Insert(Member input = null, Member output = null)
        {
            var result = output ?? MemberFake.WithoutGuild().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.InsertAsync(It.IsAny<Member>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.InsertAsync(input, default)).ReturnsAsync(result);
            }

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

        public MemberRepositoryMockBuilder Paginate(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var PagedResponse = PagedResponseFake.PaginateMembers(pageSize, page, totalItems);

            _mock.Setup(x => x.PaginateAsync(It.IsAny<Expression<Func<Member, bool>>>(), pageSize, page, default))
                 .ReturnsAsync(PagedResponse);

            return this;
        }

        public IMemberRepository Build()
        {
            return _mock.Object;
        }
    }
}
