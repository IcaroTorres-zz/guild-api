using Domain.Common;
using Domain.Models;
using Moq;

namespace Tests.Helpers.Builders
{
    public sealed class ModelFactoryMockBuilder
    {
        private readonly Mock<IModelFactory> _mock;

        private ModelFactoryMockBuilder()
        {
            _mock = new Mock<IModelFactory>();
        }

        public static ModelFactoryMockBuilder Create()
        {
            return new ModelFactoryMockBuilder();
        }

        public ModelFactoryMockBuilder CreateMember(string name, Member model)
        {
            _mock.Setup(x => x.CreateMember(name)).Returns(model);
            return this;
        }

        public ModelFactoryMockBuilder CreateGuild(string name, Member member, Guild model)
        {
            _mock.Setup(x => x.CreateGuild(name, member)).Returns(model);
            return this;
        }

        public ModelFactoryMockBuilder CreateInvite(Guild guild, Member member, Invite model)
        {
            _mock.Setup(x => x.CreateInvite(guild, member)).Returns(model);
            return this;
        }

        public ModelFactoryMockBuilder CreateMembership(Guild guild, Member member, Membership model)
        {
            _mock.Setup(x => x.CreateMembership(guild, member)).Returns(model);
            return this;
        }

        public IModelFactory Build()
        {
            return _mock.Object;
        }
    }
}
