using Domain.Repositories;
using Domain.Unities;
using Moq;
using System;

namespace Tests.Helpers.Builders
{
    public sealed class UnitOfWorkMockBuilder
    {
        private readonly Mock<IUnitOfWork> _mock;

        private UnitOfWorkMockBuilder()
        {
            _mock = new Mock<IUnitOfWork>();
        }

        public static UnitOfWorkMockBuilder Create()
        {
            return new UnitOfWorkMockBuilder();
        }

        public UnitOfWorkMockBuilder SetupRepository(IGuildRepository repository)
        {
            _mock.SetupGet(x => x.Guilds).Returns(repository);

            return this;
        }

        public UnitOfWorkMockBuilder SetupGuilds(Func<GuildRepositoryMockBuilder, IGuildRepository> repositoryConfig)
        {
            var repositoryMockBuilder = GuildRepositoryMockBuilder.Create();
            _mock.SetupGet(x => x.Guilds).Returns(repositoryConfig(repositoryMockBuilder));

            return this;
        }

        public UnitOfWorkMockBuilder SetupRepository(IMemberRepository repository)
        {
            _mock.SetupGet(x => x.Members).Returns(repository);

            return this;
        }

        public UnitOfWorkMockBuilder SetupMembers(Func<MemberRepositoryMockBuilder, IMemberRepository> repositoryConfig)
        {
            var repositoryMockBuilder = MemberRepositoryMockBuilder.Create();
            _mock.SetupGet(x => x.Members).Returns(repositoryConfig(repositoryMockBuilder));

            return this;
        }

        public UnitOfWorkMockBuilder SetupRepository(IInviteRepository repository)
        {
            _mock.SetupGet(x => x.Invites).Returns(repository);

            return this;
        }

        public UnitOfWorkMockBuilder SetupInvites(Func<InviteRepositoryMockBuilder, IInviteRepository> repositoryConfig)
        {
            var repositoryMockBuilder = InviteRepositoryMockBuilder.Create();
            _mock.SetupGet(x => x.Invites).Returns(repositoryConfig(repositoryMockBuilder));

            return this;
        }

        public UnitOfWorkMockBuilder SetupRepository(IMembershipRepository repository)
        {
            _mock.SetupGet(x => x.Memberships).Returns(repository);

            return this;
        }

        public UnitOfWorkMockBuilder SetupMemberships(Func<MembershipRepositoryMockBuilder, IMembershipRepository> repositoryConfig)
        {
            var repositoryMockBuilder = MembershipRepositoryMockBuilder.Create();
            _mock.SetupGet(x => x.Memberships).Returns(repositoryConfig(repositoryMockBuilder));

            return this;
        }

        public IUnitOfWork Build()
        {
            return _mock.Object;
        }
    }
}
