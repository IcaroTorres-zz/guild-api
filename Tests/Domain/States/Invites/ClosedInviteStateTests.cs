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
    public class CosedInviteStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_GivenStatus()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Denied;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();

            // act
            var sut = new ClosedInviteState(invite, initStatus);

            // assert
            sut.Status.Should().Be(initStatus).And.Be(invite.Status);
        }

        [Fact]
        public void BeAccepted_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Denied;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.State;
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.BeAccepted(factory);

            // assert
            sut.Status.Should().Be(initStatus).And.Be(invite.Status);
            invite.Guild.Should().NotBeOfType<NullGuild>();
            invite.Member.Should().NotBeOfType<NullMember>();
            invite.Guild.Members.Should().NotContain(invite.Member);
            invite.Member.Guild.Should().NotBe(invite.Guild);
        }

        [Fact]
        public void BeDenied_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Denied;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.State;

            // act
            sut.BeDenied();

            // assert
            sut.Status.Should().Be(initStatus).And.Be(invite.Status);
            invite.Guild.Should().NotBeOfType<NullGuild>();
            invite.Member.Should().NotBeOfType<NullMember>();
            invite.Guild.Members.Should().NotContain(invite.Member);
            invite.Member.Guild.Should().NotBe(invite.Guild);
        }

        [Fact]
        public void BeCanceled_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses initStatus = InviteStatuses.Denied;
            var invite = InviteFake.ValidWithStatus(initStatus).Generate();
            var sut = invite.State;

            // act
            sut.BeCanceled();

            // assert
            sut.Status.Should().Be(initStatus).And.Be(invite.Status);
            invite.Guild.Should().NotBeOfType<NullGuild>();
            invite.Member.Should().NotBeOfType<NullMember>();
            invite.Guild.Members.Should().NotContain(invite.Member);
            invite.Member.Guild.Should().NotBe(invite.Guild);
        }
    }
}