using Domain.Enums;
using Domain.Nulls;
using Domain.States.Invites;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.States.Invites
{
    [Trait("Domain", "Model-State")]
    public class OpenInviteStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_StatusPending()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Pending;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();

            // act
            var sut = new OpenInviteState(invite);

            // assert
            sut.Status.Should().Be(initStatus).And.Be(invite.Status);
        }

        [Fact]
        public void BeAccepted_Should_Change_StatusAccepted_MemberGuild_GuildMembers()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Pending;
            const InviteStatuses expectedStatus = InviteStatuses.Accepted;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.GetState();
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.BeAccepted(factory);

            // assert
            sut.Status.Should().Be(initStatus).And.NotBe(invite.Status);
            invite.Status.Should().Be(expectedStatus);
            invite.GetGuild().Should().NotBeOfType<NullGuild>();
            invite.GetMember().Should().NotBeOfType<NullMember>();
            invite.GetGuild().Members.Should().Contain(invite.GetMember());
            invite.GetMember().GetGuild().Should().Be(invite.GetGuild());
        }

        [Fact]
        public void BeDenied_Should_Change_StatusDenied_NotChange_MemberGuild_GuildMembers()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Pending;
            const InviteStatuses expectedStatus = InviteStatuses.Denied;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.GetState();

            // act
            sut.BeDenied();

            // assert
            sut.Status.Should().Be(initStatus).And.NotBe(invite.Status);
            invite.Status.Should().Be(expectedStatus);
            invite.GetGuild().Should().NotBeOfType<NullGuild>();
            invite.GetMember().Should().NotBeOfType<NullMember>();
            invite.GetGuild().Members.Should().NotContain(invite.GetMember());
            invite.GetMember().GetGuild().Should().NotBe(invite.GetGuild());
        }

        [Fact]
        public void BeCanceled_Should_Change_StatusCanceled_NotChange_MemberGuild_GuildMembers()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Pending;
            const InviteStatuses expectedStatus = InviteStatuses.Canceled;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.GetState();

            // act
            sut.BeCanceled();

            // assert
            sut.Status.Should().Be(initStatus).And.NotBe(invite.Status);
            invite.Status.Should().Be(expectedStatus);
            invite.GetGuild().Should().NotBeOfType<NullGuild>();
            invite.GetMember().Should().NotBeOfType<NullMember>();
            invite.GetGuild().Members.Should().NotContain(invite.GetMember());
            invite.GetMember().GetGuild().Should().NotBe(invite.GetGuild());
        }
    }
}