using Domain.Enums;
using Domain.Models;
using Domain.Nulls;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.Proxies;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class InviteTests
    {
        [Fact]
        public void BeCanceled_Not_Status_Pending_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses expectedStatus = InviteStatuses.Accepted;
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(expectedStatus).Generate();
            var monitor = sut.Monitor();

            // act
            sut.BeCanceled();

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member),
                nameof(Invite.Status));
        }

        [Fact]
        public void BeCanceled_Status_Pending_Should_Change_Status()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();

            // act
            sut.BeCanceled();

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Canceled);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));
        }
        [Fact]
        public void BeDenied_Not_Status_Pending_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses expectedStatus = InviteStatuses.Accepted;
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(expectedStatus).Generate();
            var monitor = sut.Monitor();

            // act
            sut.BeDenied();

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member),
                nameof(Invite.Status));
        }

        [Fact]
        public void BeDenied_Status_Pending_Should_Change_Status()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();

            // act
            sut.BeDenied();

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Denied);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));
        }

        [Fact]
        public void BeAccepted_Status_Pending_ValidGuild_ValidMember_Should_Change_Status()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();
            var guildMonitor = ((GuildTestProxy)sut.GetGuild()).Monitor();
            var memberMonitor = ((MemberTestProxy)sut.GetMember()).Monitor();
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.BeAccepted(factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Accepted);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));

            guildMonitor.AssertCollectionChanged(sut.GetGuild().Members);
            memberMonitor.AssertCollectionChanged(sut.GetMember().Memberships);
            memberMonitor.AssertPropertyChanged(nameof(Guild), nameof(Member.GuildId));
        }

        [Fact]
        public void BeAccepted_Status_Not_Pending_ValidGuild_ValidMember_Should_Change_Nothing()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.ValidWithStatus(InviteStatuses.Canceled).Generate();
            var monitor = sut.Monitor();
            var guildMonitor = ((GuildTestProxy)sut.GetGuild()).Monitor();
            var memberMonitor = ((MemberTestProxy)sut.GetMember()).Monitor();
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.BeAccepted(factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Canceled);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));

            guildMonitor.AssertCollectionNotChanged(sut.GetGuild().Members);
            memberMonitor.AssertCollectionNotChanged(sut.GetMember().Memberships);
            memberMonitor.AssertPropertyNotChanged(nameof(Guild), nameof(Member.GuildId));
        }

        [Fact]
        public void BeAccepted_Status_Pending_WithoutGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.InvalidWithoutGuild().Generate();
            var expectedStatus = sut.Status;
            var monitor = sut.Monitor();
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.BeAccepted(factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull().And.BeOfType<NullGuild>();
            sut.GuildId.Should().BeNull();
            sut.GetMember().Should().NotBeNull().And.BeOfType<MemberTestProxy>();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));
        }

        [Fact]
        public void BeAccepted_Status_Pending_WithoutMember_Should_Change_Nothing()
        {
            // arrange
            var sut = (InviteTestProxy)InviteFake.InvalidWithoutMember().Generate();
            var monitor = sut.Monitor();

            // act
            sut.BeAccepted(TestModelFactoryHelper.Factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            sut.Id.Should().NotBeEmpty();
            sut.GetGuild().Should().NotBeNull().And.BeOfType<GuildTestProxy>();
            sut.GuildId.Should().NotBeEmpty();
            sut.GetMember().Should().NotBeNull().And.BeOfType<NullMember>();
            sut.MemberId.Should().BeNull();
            sut.Status.Should().NotBe(InviteStatuses.Accepted);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Guild),
                nameof(Invite.MemberId),
                nameof(Member));
        }
    }
}
