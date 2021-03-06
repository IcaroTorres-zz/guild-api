using Application.Common.Abstractions;
using Domain.Models;
using Moq;
using System;
using Tests.Domain.Models.Fakes;

namespace Tests.Helpers.Builders
{
    public sealed class GuildRepositoryMockBuilder
    {
        private readonly Mock<IGuildRepository> _mock;

        private GuildRepositoryMockBuilder()
        {
            _mock = new Mock<IGuildRepository>();
        }

        public static GuildRepositoryMockBuilder Create()
        {
            return new GuildRepositoryMockBuilder();
        }

        public GuildRepositoryMockBuilder ExistsWithName(bool result, string name = null)
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

        public GuildRepositoryMockBuilder GetByIdSuccess(Guid input, Guild output = null)
        {
            var result = output ?? GuildFake.Complete(membersCount: 3).Generate();

            _mock.Setup(x => x.GetByIdAsync(input, It.IsAny<bool>(), default)).ReturnsAsync(result);

            return this;
        }

        public GuildRepositoryMockBuilder GetByIdFail(Guid? id = null)
        {
            var result = Guild.Null;

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

        public GuildRepositoryMockBuilder ExistsWithId(bool result, Guid? id = null)
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

        public GuildRepositoryMockBuilder GetForMemberHandlingSuccess(Guid? input = null, Guild output = null)
        {
            var result = output ?? GuildFake.Complete(membersCount: 3).Generate();

            if (input == null)
            {
                _mock.Setup(x => x.GetForMemberHandlingAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetForMemberHandlingAsync(input.Value, default)).ReturnsAsync(result);
            }

            return this;
        }

        public GuildRepositoryMockBuilder GetForMemberHandlingFail(Guid? input = null)
        {
            var result = Guild.Null;

            if (input == null)
            {
                _mock.Setup(x => x.GetForMemberHandlingAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetForMemberHandlingAsync(input.Value, default)).ReturnsAsync(result);
            }

            return this;
        }

        public GuildRepositoryMockBuilder CanChangeName(bool result, Guid? id = null, string name = null)
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

        public GuildRepositoryMockBuilder Insert(Guild input = null, Guild output = null)
        {
            var result = output ?? GuildFake.Complete().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.InsertAsync(It.IsAny<Guild>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.InsertAsync(input, default)).ReturnsAsync(result);
            }

            return this;
        }

        public GuildRepositoryMockBuilder Update(Guild input = null, Guild output = null)
        {
            var result = output ?? GuildFake.Complete().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.Update(It.IsAny<Guild>())).Returns(result);
            }
            else
            {
                _mock.Setup(x => x.Update(input)).Returns(result);
            }

            return this;
        }

        public GuildRepositoryMockBuilder Paginate(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var PagedResponse = PagedResponseFake.PaginateGuilds(pageSize, page, totalItems);

            _mock.Setup(x => x.PaginateAsync(pageSize, page, default)).ReturnsAsync(PagedResponse);

            return this;
        }

        public IGuildRepository Build()
        {
            return _mock.Object;
        }
    }
}
