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
		public class GuildTests
		{
		[Fact]
		public void Constructor_WithNameAndMember_Should_CreateWith_AllProperties()
		{
			// arrange
			const string expectedName = "name";
			var member = MemberFake.WithoutGuild().Generate();

			// act
			var sut = new Guild(expectedName, member);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Id.Should().NotBeEmpty();
			sut.Name.Should().NotBeEmpty().And.Be(expectedName);
			sut.Members.Should().Contain(member);
			sut.Invites.Should().Contain(x => x.MemberId == member.Id && x.GuildId == sut.Id);
		}

		[Fact]
		public void ChangeName_WithName_Should_Change_NameOnly()
		{
			// arrange
			const string expectedNane = "new name";
			var sut = GuildFake.WithGuildLeader().Generate();
			var monitor = sut.Monitor();

			// act
			sut.ChangeName(expectedNane);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Name.Should().NotBeEmpty().And.Be(expectedNane);

			monitor.AssertPropertyChanged(nameof(Guild.Name));
			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Members),
				nameof(Guild.Invites));
		}

		[Fact]
		public void RemoveMember_GuildMember_Should_Change_Guild()
		{
			// arrange
			const int otherMembersCount = 10;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var member = sut.Vice;
			var memberMonitor = member.Monitor();

			// act
			sut = sut.RemoveMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount)
				.And.NotContain(member);
			sut.Invites.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1);

			monitor.AssertCollectionChanged(sut.Members);
			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites);

			member.Should().NotBeNull().And.BeOfType<Member>();
			member.IsGuildLeader.Should().BeFalse();
			member.GuildId.Should().BeNull();
			member.Guild.Should().BeOfType<NullGuild>();

			memberMonitor.AssertPropertyChanged(
				nameof(Member.GuildId),
				nameof(Member.Guild));
			memberMonitor.AssertPropertyNotChanged(
				nameof(Member.Id),
				nameof(Member.Name),
				nameof(Member.IsGuildLeader));
			memberMonitor.AssertCollectionNotChanged(member.Memberships);
		}

		[Fact]
		public void RemoveMember_GuildLeader_Should_Change_Guild_Member()
		{
			// arrange
			const int otherMembersCount = 10;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var master = sut.Leader;
			var memberMonitor = master.Monitor();

			// act
			sut = sut.RemoveMember(master);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount)
				.And.NotContain(master);
			sut.Invites.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1);

			monitor.AssertCollectionChanged(sut.Members);
			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites);

			master.Should().NotBeNull()
				.And.BeOfType<Member>();
			master.IsGuildLeader.Should().BeFalse();
			master.GuildId.Should().BeNull();
			master.Guild.Should().BeOfType<NullGuild>();

			memberMonitor.AssertPropertyChanged(
				nameof(Member.IsGuildLeader),
				nameof(Member.GuildId),
				nameof(Member.Guild));
			memberMonitor.AssertPropertyNotChanged(
				nameof(Member.Id),
				nameof(Member.Name));
			memberMonitor.AssertCollectionNotChanged(master.Memberships);
		}

		[Fact]
		public void RemoveMember_WithoutGuild_Should_Change_Nothing()
		{
			// arrange
			const int otherMembersCount = 10;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var member = MemberFake.WithoutGuild().Generate();
			var memberMonitor = member.Monitor();

			// act
			sut = sut.RemoveMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1)
				.And.NotContain(member);
			sut.Invites.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name),
				nameof(Guild.Members),
				nameof(Guild.Invites));

			member.Should().NotBeNull()
				.And.BeOfType<Member>();
			member.IsGuildLeader.Should().BeFalse();
			member.GuildId.Should().BeNull();
			member.Guild.Should().BeOfType<NullGuild>();

			memberMonitor.AssertPropertyNotChanged(
				nameof(Member.Id),
				nameof(Member.Name),
				nameof(Member.GuildId),
				nameof(Member.Guild),
				nameof(Member.IsGuildLeader),
				nameof(Member.Memberships));
		}

		[Fact]
		public void RemoveMember_OtherGuildMember_Should_Change_Nothing()
		{
			// arrange
			const int otherMembersCount = 10;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var otherGuild = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var member = otherGuild.Members.Single(x => !x.IsGuildLeader);
			var memberMonitor = member.Monitor();

			// act
			sut = sut.RemoveMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1)
				.And.NotContain(member);
			sut.Invites.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name),
				nameof(Guild.Members),
				nameof(Guild.Invites));

			member.Should().NotBeNull().And.BeOfType<Member>();
			member.IsGuildLeader.Should().BeFalse();

			memberMonitor.AssertPropertyNotChanged(
				nameof(Member.Id),
				nameof(Member.Name),
				nameof(Member.GuildId),
				nameof(Member.Guild),
				nameof(Member.IsGuildLeader),
				nameof(Member.Memberships));
		}

		[Fact]
		public void RemoveMember_OtherGuildLeader_Should_Change_Nothing()
		{
			// arrange
			const int otherMembersCount = 10;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var otherGuild = GuildFake.WithGuildLeader().Generate();
			var member = otherGuild.Members.First();
			var memberMonitor = member.Monitor();

			// act
			sut = sut.RemoveMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1)
				.And.NotContain(member);
			sut.Invites.Should().NotBeEmpty()
				.And.HaveCount(otherMembersCount + 1);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name),
				nameof(Guild.Members),
				nameof(Guild.Invites));

			member.Should().NotBeNull().And.BeOfType<Member>();

			memberMonitor.AssertPropertyNotChanged(
				nameof(Member.Id),
				nameof(Member.Name),
				nameof(Member.GuildId),
				nameof(Member.Guild),
				nameof(Member.IsGuildLeader),
				nameof(Member.Memberships));
		}

		[Fact]
		public void AddMember_NullMemberModel_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeader().Generate();
			var monitor = sut.Monitor();
			var member = MemberFake.NullObject().Generate();

			// act
			sut = sut.AddMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty();
			sut.Invites.Should().NotBeEmpty();
			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
		}

		[Fact]
		public void AddMember_AlreadyInGuild_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var member = sut.Members.First();

			// act
			sut = sut.AddMember(member);

			// assert
			sut.Should().NotBeNull()
				.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty();
			sut.Invites.Should().NotBeEmpty();
			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
		}

		[Fact]
		public void AddMember_NotInGuild_Should_Change_Members()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var member = MemberFake.WithoutGuild().Generate();

			// act
			sut = sut.AddMember(member);

			// assert
			sut.Should().NotBeNull()
					.And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty();
			sut.Invites.Should().NotBeEmpty();

			monitor.AssertCollectionChanged(sut.Members);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites);
		}

		[Fact]
		public void InviteMember_NullMember_Should_Have_LatestInvite_EqualTo_NullInvite()
		{
			// arrange
			const int otherMembersCount = 1;
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
			var monitor = sut.Monitor();
			var member = MemberFake.NullObject().Generate();

			// act
			sut = sut.InviteMember(member);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.LatestInvite.Should().NotBeNull().And.BeOfType<NullInvite>();
			sut.Members.Should().NotBeEmpty().And.HaveCount(otherMembersCount + 1);
			sut.Invites.Should().NotBeEmpty().And.HaveCount(otherMembersCount + 2);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
		}

		[Fact]
		public void InviteMember_AlreadyInGuild_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var member = sut.Members.First();

			// act
			sut = sut.InviteMember(member);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().NotBeEmpty().And.HaveCount(2);
			sut.Invites.Should().NotBeEmpty().And.HaveCount(2);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
		}

		[Fact]
		public void InviteMember_NotInGuild_Should_Change_Invites()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var member = MemberFake.WithoutGuild().Generate();

			// act
			sut = sut.InviteMember(member);

			// assert
			
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.LatestInvite.Should().NotBeNull().And.BeOfType<Invite>();
			sut.Members.Should().NotBeEmpty().And.HaveCount(2);
			sut.Invites.Should().NotBeEmpty().And.HaveCount(3);

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionChanged(sut.Invites);
			monitor.AssertCollectionNotChanged(sut.Members);
		}

		[Fact]
		public void Promote_GuildMember_Should_Change_GuildLeader()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var newLeader = sut.Vice;
			var perviousLeader = sut.Leader;

			// act
			sut = sut.Promote(newLeader);

			// assert

			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().Contain(new[] { newLeader, perviousLeader });
			sut.Leader.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(newLeader)
				.And.NotBe(perviousLeader);
			sut.Vice.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(perviousLeader)
				.And.NotBe(newLeader);
			newLeader.IsGuildLeader.Should().BeTrue();
			perviousLeader.IsGuildLeader.Should().BeFalse();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}

		[Fact]
		public void Promote_GuildLeader_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var leader = sut.Leader;
			var vice = sut.Vice;

			// act
			sut = sut.Promote(leader);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().Contain(new []{ leader, vice });
			sut.Leader.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(leader)
				.And.NotBe(vice);
			sut.Vice.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(vice)
				.And.NotBe(leader);
			leader.IsGuildLeader.Should().BeTrue();
			vice.IsGuildLeader.Should().BeFalse();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}

		[Fact]
		public void Promote_NotMember_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var notMember = MemberFake.WithoutGuild().Generate();
			var currentLeader = sut.Leader;

			// act
			sut = sut.Promote(notMember);

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().Contain(currentLeader).And.NotContain(notMember);
			sut.Leader.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(currentLeader)
				.And.NotBe(notMember);
			sut.Vice.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.NotBe(currentLeader)
				.And.NotBe(notMember);
			notMember.IsGuildLeader.Should().BeFalse();
			currentLeader.IsGuildLeader.Should().BeTrue();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}

		[Fact]
		public void DemoteLeader_GuildWithLeaderAndMembers_Should_Change_GuildLeader()
		{
			// arrange
			var sut = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: 1).Generate();
			var monitor = sut.Monitor();
			var newLeader = sut.Vice;
			var perviousLeader = sut.Leader;

			// act
			sut = sut.DemoteLeader();

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().Contain(new[] { newLeader, perviousLeader });
			sut.Leader.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(newLeader)
				.And.NotBe(perviousLeader);
			sut.Vice.Should().NotBeNull()
				.And.NotBeOfType<NullMember>()
				.And.Be(perviousLeader)
				.And.NotBe(newLeader);
			newLeader.IsGuildLeader.Should().BeTrue();
			perviousLeader.IsGuildLeader.Should().BeFalse();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}

		[Fact]
		public void DemoteLeader_GuildWithLeaderOnly_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeader().Generate();
			var monitor = sut.Monitor();
			var leader = sut.Leader;
			var nullVice = sut.Vice;

			// act
			sut = sut.DemoteLeader();

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().Contain(leader).And.NotContain(nullVice);
			sut.Leader.Should().NotBeNull()
				.And.BeOfType<Member>()
				.And.Be(leader)
				.And.NotBe(nullVice);
			sut.Vice.Should().NotBeNull()
				.And.BeOfType<NullMember>()
				.And.Be(nullVice)
				.And.NotBe(leader);
			leader.IsGuildLeader.Should().BeTrue();
			nullVice.IsGuildLeader.Should().BeFalse();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}

		[Fact]
		public void DemoteLeader_NoMembers_Should_Change_Nothing()
		{
			// arrange
			var sut = GuildFake.WithGuildLeader().Generate();
			sut.RemoveMember(sut.Leader);
			var monitor = sut.Monitor();
			var nullLeader = sut.Leader;
			var nullVice = sut.Vice;

			// act
			sut = sut.DemoteLeader();

			// assert
			sut.Should().NotBeNull().And.BeOfType<Guild>();
			sut.Members.Should().HaveCount(0).And.NotContain(new []{nullLeader, nullVice});
			sut.Leader.Should().NotBeNull()
				.And.BeOfType<NullMember>()
				.And.Be(nullVice)
				.And.Be(nullLeader);
			sut.Vice.Should().NotBeNull()
				.And.BeOfType<NullMember>()
				.And.Be(nullVice)
				.And.Be(nullLeader);
			nullLeader.IsGuildLeader.Should().BeFalse();
			nullVice.IsGuildLeader.Should().BeFalse();

			monitor.AssertPropertyNotChanged(
				nameof(Guild.Id),
				nameof(Guild.Name));
			monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
		}
	}
}
