using Domain.Enums;
using Domain.Models;
using Domain.Nulls;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class InviteTests
    {
        [Fact]
        public void Constructor_WithGuildAndMember_Should_Create_Pending_With_AllProperties()
        {
            // arrange
            var guild = GuildFake.Valid().Generate();
            var member = MemberFake.WithoutGuild().Generate();

            // act
            var sut = new Invite(guild, member);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull().And.Be(guild);
            sut.GuildId.Should().NotBeEmpty().And.Be(guild.Id);
            sut.Member.Should().NotBeNull().And.Be(member);
            sut.MemberId.Should().NotBeEmpty().And.Be(member.Id);
            sut.Status.Should().Be(InviteStatuses.Pending);
        }

        [Fact]
        public void BeCanceled_Not_Status_Pending_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses expectedStatus = InviteStatuses.Accepted;
            var sut = InviteFake.ValidWithStatus(expectedStatus).Generate();
            var monitor = sut.Monitor();

            // act
            sut = sut.BeCanceled();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member),
                nameof(Invite.Status));
        }

        [Fact]
        public void BeCanceled_Status_Pending_Should_Change_Status()
        {
            // arrange
            var sut = InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();

            // act
            sut = sut.BeCanceled();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Canceled);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));
        }
        [Fact]
        public void BeDenied_Not_Status_Pending_Should_Change_Nothing()
        {
            // arrange
            const InviteStatuses expectedStatus = InviteStatuses.Accepted;
            var sut = InviteFake.ValidWithStatus(expectedStatus).Generate();
            var monitor = sut.Monitor();

            // act
            sut = sut.BeDenied();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member),
                nameof(Invite.Status));
        }

        [Fact]
        public void BeDenied_Status_Pending_Should_Change_Status()
        {
            // arrange
            var sut = InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();

            // act
            sut = sut.BeDenied();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Denied);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));
        }

        [Fact]
        public void BeAccepted_Status_Pending_ValidGuild_ValidMember_Should_Change_Status()
        {
            // arrange
            var sut = InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var monitor = sut.Monitor();
            var guildMonitor = sut.Guild.Monitor();
            var memberMonitor = sut.Member.Monitor();

            // act
            sut = sut.BeAccepted();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Accepted);

            monitor.AssertPropertyChanged(nameof(Invite.Status));
            monitor.AssertPropertyNotChanged(
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));

            guildMonitor.AssertCollectionChanged(sut.Guild.Members);
            memberMonitor.AssertCollectionChanged(sut.Member.Memberships);
            memberMonitor.AssertPropertyChanged(nameof(Member.Guild), nameof(Member.GuildId));
        }

        [Fact]
        public void BeAccepted_Status_Not_Pending_ValidGuild_ValidMember_Should_Change_Nothing()
        {
            // arrange
            var sut = InviteFake.ValidWithStatus(InviteStatuses.Canceled).Generate();
            var monitor = sut.Monitor();
            var guildMonitor = sut.Guild.Monitor();
            var memberMonitor = sut.Member.Monitor();

            // act
            sut = sut.BeAccepted();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(InviteStatuses.Canceled);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));

            guildMonitor.AssertCollectionNotChanged(sut.Guild.Members);
            memberMonitor.AssertCollectionNotChanged(sut.Member.Memberships);
            memberMonitor.AssertPropertyNotChanged(nameof(Member.Guild), nameof(Member.GuildId));
        }

        [Fact]
        public void BeAccepted_Status_Pending_WithoutGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = InviteFake.InvalidWithoutGuild().Generate();
            var expectedStatus = sut.Status;
            var monitor = sut.Monitor();

            // act
            sut = sut.BeAccepted();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>();
            sut.GuildId.Should().BeEmpty();
            sut.Member.Should().NotBeNull().And.BeOfType<Member>();
            sut.MemberId.Should().NotBeEmpty();
            sut.Status.Should().Be(expectedStatus);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));
        }

        [Fact]
        public void BeAccepted_Status_Pending_WithoutMember_Should_Change_Nothing()
        {
            // arrange
            var sut = InviteFake.InvalidWithoutMember().Generate();
            var monitor = sut.Monitor();

            // act
            sut = sut.BeAccepted();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Invite>();
            sut.Id.Should().NotBeEmpty();
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>();
            sut.GuildId.Should().NotBeEmpty();
            sut.Member.Should().NotBeNull().And.BeOfType<NullMember>();
            sut.MemberId.Should().BeEmpty();
            sut.Status.Should().NotBe(InviteStatuses.Accepted);

            monitor.AssertPropertyNotChanged(
                nameof(Invite.Status),
                nameof(Invite.Id),
                nameof(Invite.GuildId),
                nameof(Invite.Guild),
                nameof(Invite.MemberId),
                nameof(Invite.Member));
        }
    }
}
