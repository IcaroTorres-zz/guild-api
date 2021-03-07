using Domain.Enums;
using Domain.Nulls;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Factories
{
    [Trait("Domain", "Model-Factory")]
    public class TestModelFactoryTests
    {
        private readonly TestModelFactory _sut;

        public TestModelFactoryTests()
        {
            _sut = TestModelFactoryHelper.Factory;
        }

        [Fact]
        public void CreateGuild_WithNameAndMember_Should_Create_With_AllProperties()
        {
            // arrange
            const string expectedName = "name";
            var member = MemberFake.WithoutGuild().Generate();

            // act
            var model = _sut.CreateGuild(expectedName, member);

            // assert
            model.Should().NotBeNull().And.BeOfType<TestGuild>();
            model.Id.Should().NotBeEmpty();
            model.Name.Should().NotBeEmpty().And.Be(expectedName);
            model.Members.Should().Contain(member);
            model.Invites.Should().Contain(x => x.MemberId == member.Id && x.GuildId == model.Id);
        }

        [Fact]
        public void CreateInvite_WithGuildAndMember_Should_Create_Pending_With_AllProperties()
        {
            // arrange
            var guild = GuildFake.Complete().Generate();
            var member = MemberFake.WithoutGuild().Generate();
            const InviteStatuses expectedStatus = InviteStatuses.Pending;

            // act
            var model = _sut.CreateInvite(guild, member);

            // assert
            model.Should().NotBeNull().And.BeOfType<TestInvite>();
            model.Id.Should().NotBeEmpty();
            model.GetGuild().Should().NotBeNull().And.Be(guild);
            model.GuildId.Should().NotBeEmpty().And.Be(guild.Id);
            model.GetMember().Should().NotBeNull().And.Be(member);
            model.MemberId.Should().NotBeEmpty().And.Be(member.Id);
            model.Status.Should().Be(expectedStatus);
        }

        [Fact]
        public void CreateMembership_WithGuildAndMember_Should_CreateWith_AllProperties_Except_ModifiedDate()
        {
            // arrange
            var guild = GuildFake.Complete().Generate();
            var member = MemberFake.WithoutGuild().Generate();

            // act
            var model = _sut.CreateMembership(guild, member);

            // assert
            model.Should().NotBeNull().And.BeOfType<TestMembership>();
            model.Id.Should().NotBeEmpty();
            model.ModifiedDate.Should().BeNull();
            model.GuildName.Should().Be(guild.Name);
            model.GuildId.Should().NotBeEmpty().And.Be(guild.Id);
            model.MemberName.Should().Be(member.Name);
            model.MemberId.Should().NotBeEmpty().And.Be(member.Id);
        }


        [Fact]
        public void Constructor_WithName_Should_CreateWith_Name_Id()
        {
            // arrange
            const string expectedName = "testname";

            // act
            var model = _sut.CreateMember(expectedName);

            // assert
            model.Should().NotBeNull().And.BeOfType<TestMember>();
            model.Id.Should().NotBeEmpty();
            model.Name.Should().NotBeEmpty().And.Be(expectedName);
            model.IsGuildLeader.Should().BeFalse();
            model.GuildId.Should().BeNull();
            model.GetGuild().Should().BeOfType<NullGuild>();
            model.Memberships.Should().BeEmpty();
        }
    }
}
