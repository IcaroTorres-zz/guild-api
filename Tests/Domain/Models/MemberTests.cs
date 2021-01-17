using Bogus;
using Domain.Models;
using Domain.Models.Nulls;
using FluentAssertions;
using System.Linq;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class MemberTests
    {
        [Fact]
        public void Constructor_WithName_Should_CreateWith_Name_Id()
        {
            // arrange
            var expectedName = new Person().UserName;

            // act
            var sut = new Member(expectedName);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.Id.Should().NotBeEmpty();
            sut.Name.Should().NotBeEmpty().And.Be(expectedName);
            sut.IsGuildLeader.Should().BeFalse();
            sut.GuildId.Should().BeNull();
            sut.Guild.Should().BeOfType<NullGuild>();
            sut.Memberships.Should().BeEmpty();
        }

        [Fact]
        public void ChangeName_WithName_Should_Change_NameOnly()
        {
            // arrange
            var expectedName = new Person().UserName;
            var sut = MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ChangeName(expectedName);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.Name.Should().NotBeEmpty().And.Be(expectedName);

            monitor.AssertPropertyChanged(nameof(Member.Name));
            monitor.AssertCollectionNotChanged(sut.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId),
                nameof(Member.Guild));
        }

        [Fact]
        public void LeaveGuild_WithoutGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.LeaveGuild();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().BeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>();

            monitor.AssertCollectionNotChanged(sut.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Guild),
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId));
        }

        [Fact]
        public void LeaveGuild_WithGuild_Should_Change_Guild_GuildId_Memberships()
        {
            // arrange
            var sut = MemberFake.GuildMember().Generate();
            var monitor = sut.Monitor();
            var membership = sut.Memberships
                .OrderByDescending(x => x.CreatedDate)
                .First(x => x.GuildId == sut.GuildId);
            var membershipMonitor = membership.Monitor();

            // act
            sut.LeaveGuild();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().BeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().BeOfType<NullGuild>();
            sut.Memberships.Should().Contain(x => x.Id == membership.Id);

            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));

            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }

        [Fact]
        public void LeaveGuild_GuildLeader_Should_Change_IsGuildLeader_Guild_GuildId_Memberships()
        {
            // arrange
            var sut = MemberFake.GuildLeader().Generate();
            var monitor = sut.Monitor();
            var membership = sut.Memberships
                .OrderByDescending(x => x.CreatedDate)
                .First(x => x.GuildId == sut.GuildId);
            var membershipMonitor = membership.Monitor();

            // act
            sut.LeaveGuild();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().BeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().BeOfType<NullGuild>();
            sut.Memberships.Should().Contain(x => x.Id == membership.Id);

            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name));

            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }

        [Fact]
        public void ReceiveLeadershipFrom_WithoutGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ReceiveLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().BeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>();

            monitor.AssertCollectionNotChanged(sut.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void ReceiveLeadershipFrom_GuildLeader_Should_Change_Nothing()
        {
            // arrange
            var sut = MemberFake.GuildLeader().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ReceiveLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeTrue();
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>();

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void ReceiveLeadershipFrom_GuildMember_Should_Change_IsGuildLeader_True()
        {
            // arrange
            var sut = MemberFake.GuildMember().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ReceiveLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeTrue();
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>();

            monitor.AssertPropertyChanged(nameof(Member.IsGuildLeader));
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.Memberships));
        }

        [Fact]
        public void BeDemoted_WithoutGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.TransferLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().BeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>();

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void BeDemoted_GuildLeader_Should_Change_IsGuildLeader_False()
        {
            // arrange
            var sut = MemberFake.GuildLeader().Generate();
            var monitor = sut.Monitor();

            // act
            sut.TransferLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>();

            monitor.AssertPropertyChanged(nameof(Member.IsGuildLeader));
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.Memberships));
        }

        [Fact]
        public void BeDemoted_GuildMember_Should_Change_Nothing()
        {
            // arrange
            var sut = MemberFake.GuildMember().Generate();
            var monitor = sut.Monitor();

            // act
            sut.TransferLeadership(MemberFake.NullObject().Generate());

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>();

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void JoingGuild_WithoutGuild_Should_Change_Guild_GuildId_Memberships()
        {
            // arrange
            var sut = MemberFake.WithoutGuild().Generate();
            var guild = GuildFake.WithGuildLeader().Generate();
            var monitor = sut.Monitor();

            // act
            sut.JoinGuild(guild);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.Be(guild);

            monitor.AssertCollectionChanged(sut.Memberships);
            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void JoingGuild_GuildMember_Should_Change_Guild_GuildId_Memberships()
        {
            // arrange
            var sut = MemberFake.GuildMember().Generate();
            var guild = GuildFake.WithGuildLeader().Generate();
            var monitor = sut.Monitor();
            var membership = sut.Memberships
                .OrderByDescending(x => x.CreatedDate)
                .First(x => x.GuildId == sut.GuildId);
            var membershipMonitor = membership.Monitor();

            // act
            sut.JoinGuild(guild);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.Be(guild);

            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
            
            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }

        [Fact]
        public void JoingGuild_GuildLeader_Should_Change_IsGuildLeader_Guild_GuildId_Memberships()
        {
            // arrange
            var sut = MemberFake.GuildLeader().Generate();
            var guild = GuildFake.WithGuildLeader().Generate();
            var monitor = sut.Monitor();
            var membership = sut.Memberships
                .OrderByDescending(x => x.CreatedDate)
                .First(x => x.GuildId == sut.GuildId);
            var membershipMonitor = membership.Monitor();

            // act
            sut.JoinGuild(guild);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.GuildId.Should().NotBeNull();
            sut.IsGuildLeader.Should().BeFalse();
            sut.Guild.Should().NotBeNull().And.Be(guild);

            monitor.AssertCollectionChanged(sut.Memberships);
            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name));

            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }
    }
}
