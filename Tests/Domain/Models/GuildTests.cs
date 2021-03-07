using Domain.Models;
using Domain.Nulls;
using FluentAssertions;
using System.Linq;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class GuildTests
    {
        [Fact]
        public void ChangeName_WithName_Should_Change_NameOnly()
        {
            // arrange
            const string expectedNane = "new name";
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ChangeName(expectedNane);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
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
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count - 1;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = (TestMember)sut.GetVice();
            var memberMonitor = member.Monitor();

            // act
            sut.RemoveMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(member);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertCollectionChanged(sut.Members);
            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites);

            member.Should().NotBeNull().And.BeOfType<TestMember>();
            member.IsGuildLeader.Should().BeFalse();
            member.GuildId.Should().BeNull();
            member.GetGuild().Should().BeOfType<NullGuild>();

            memberMonitor.AssertPropertyChanged(
                nameof(Member.GuildId),
                nameof(Guild));
            memberMonitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
            memberMonitor.AssertCollectionNotChanged(member.Memberships);
        }

        [Fact]
        public void RemoveMember_GuildLeader_Should_Change_Guild_Members()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count - 1;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var leader = (TestMember)sut.GetLeader();
            var memberMonitor = leader.Monitor();

            // act
            sut.RemoveMember(leader);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(leader);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertCollectionChanged(sut.Members);
            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites);

            leader.Should().NotBeNull()
                .And.BeOfType<TestMember>();
            leader.IsGuildLeader.Should().BeFalse();
            leader.GuildId.Should().BeNull();
            leader.GetGuild().Should().BeOfType<NullGuild>();

            memberMonitor.AssertPropertyChanged(
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId),
                nameof(Guild));
            memberMonitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name));
            memberMonitor.AssertCollectionNotChanged(leader.Memberships);
        }

        [Fact]
        public void RemoveMember_WithoutGuild_Should_Change_Nothing()
        {
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = (TestMember)MemberFake.WithoutGuild().Generate();
            var memberMonitor = member.Monitor();

            // act
            sut.RemoveMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(member);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name),
                nameof(Guild.Members),
                nameof(Guild.Invites));

            member.Should().NotBeNull()
                .And.BeOfType<TestMember>();
            member.IsGuildLeader.Should().BeFalse();
            member.GuildId.Should().BeNull();
            member.GetGuild().Should().BeOfType<NullGuild>();

            memberMonitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.GuildId),
                nameof(Guild),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void RemoveMember_OtherGuildMember_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var otherGuild = (TestGuild)GuildFake.Complete(membersCount: 1).Generate();
            var member = (TestMember)otherGuild.Members.Single(x => !x.IsGuildLeader);
            var memberMonitor = member.Monitor();

            // act
            sut.RemoveMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(member);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name),
                nameof(Guild.Members),
                nameof(Guild.Invites));

            member.Should().NotBeNull().And.BeOfType<TestMember>();
            member.IsGuildLeader.Should().BeFalse();

            memberMonitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.GuildId),
                nameof(Guild),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void RemoveMember_OtherGuildLeader_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var otherGuild = (TestGuild)GuildFake.Complete().Generate();
            var member = (TestMember)otherGuild.Members.First();
            var memberMonitor = member.Monitor();

            // act
            sut.RemoveMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(member);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name),
                nameof(Guild.Members),
                nameof(Guild.Invites));

            member.Should().NotBeNull().And.BeOfType<TestMember>();

            memberMonitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.GuildId),
                nameof(Guild),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void RemoveMember_NullMember_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = Member.Null;

            // act
            sut.RemoveMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.NotContain(member);
            sut.Invites.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name),
                nameof(Guild.Members),
                nameof(Guild.Invites));
        }

        [Fact]
        public void AddMember_NullMemberModel_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = Member.Null;

            // act
            sut.AddMember(member);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);
            sut.Invites.Should().NotBeEmpty().And.HaveCount(expectedInvitesCount);
            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
        }

        [Fact]
        public void AddMember_AlreadyInGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = sut.Members.First();

            // act
            sut.AddMember(member);

            // assert
            sut.Should().NotBeNull()
                .And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);
            sut.Invites.Should().NotBeEmpty().And.HaveCount(expectedInvitesCount);
            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
        }

        [Fact]
        public void AddMember_NotInGuild_Should_Change_Members()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count + 1;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = (TestMember)MemberFake.WithoutGuild().Generate();

            // act
            sut.AddMember(member);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);
            sut.Invites.Should().NotBeEmpty().And.HaveCount(expectedInvitesCount);

            monitor.AssertCollectionChanged(sut.Members);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites);
        }

        [Fact]
        public void InviteMember_NullMember_ShouldNot_GenerateInvite()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = Member.Null;

            // act
            sut.InviteMember(member, TestModelFactoryHelper.Factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Invites.Should().NotBeEmpty()
                .And.AllBeOfType<TestInvite>()
                .And.Match(x => !x.Any(y => y.GetMember().Equals(member)))
                .And.HaveCount(expectedMembersCount);
            sut.Members.Should().NotBeEmpty()
                .And.HaveCount(expectedInvitesCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
        }

        [Fact]
        public void InviteMember_AlreadyInGuild_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count;
            var monitor = sut.Monitor();
            var member = sut.Members.First();

            // act
            sut.InviteMember(member, TestModelFactoryHelper.Factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);
            sut.Invites.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Invites, sut.Members);
        }

        [Fact]
        public void InviteMember_NotInGuild_Should_Change_Invites()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var expectedMembersCount = sut.Members.Count;
            var expectedInvitesCount = sut.Invites.Count + 1;
            var monitor = sut.Monitor();
            var member = (TestMember)MemberFake.WithoutGuild().Generate();

            // act
            sut.InviteMember(member, TestModelFactoryHelper.Factory);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.GetLatestInvite().Should().NotBeNull().And.BeOfType<TestInvite>();
            sut.Members.Should().NotBeEmpty().And.HaveCount(expectedMembersCount);
            sut.Invites.Should().NotBeEmpty()
                .And.Match(x => x.Any(y => y.GetMember().Equals(member)))
                .And.HaveCount(expectedInvitesCount);

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
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var monitor = sut.Monitor();
            var newLeader = sut.GetVice();
            var perviousLeader = sut.GetLeader();

            // act
            sut.Promote(newLeader);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().Contain(new[] { newLeader, perviousLeader });
            sut.GetLeader().Should().NotBeNull()
                .And.NotBeOfType<NullMember>()
                .And.Be(newLeader)
                .And.NotBe(perviousLeader);
            sut.GetVice().Should().NotBeNull()
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
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var monitor = sut.Monitor();
            var leader = sut.GetLeader();
            var vice = sut.GetVice();

            // act
            sut.Promote(leader);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().Contain(new[] { leader, vice });
            sut.GetLeader().Should().NotBeNull()
                .And.NotBeOfType<NullMember>()
                .And.Be(leader)
                .And.NotBe(vice);
            sut.GetVice().Should().NotBeNull()
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
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var monitor = sut.Monitor();
            var notMember = (TestMember)MemberFake.WithoutGuild().Generate();
            var unchangedLeader = sut.GetLeader();

            // act
            sut.Promote(notMember);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().Contain(unchangedLeader).And.NotContain(notMember);
            sut.GetLeader().Should().NotBeNull()
                .And.BeOfType<TestMember>()
                .And.Be(unchangedLeader)
                .And.NotBe(notMember);
            sut.GetVice().Should().NotBeNull()
                .And.BeOfType<TestMember>();
            notMember.IsGuildLeader.Should().BeFalse().And.Be(!unchangedLeader.IsGuildLeader);

            monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
        }

        [Fact]
        public void DemoteLeader_GuildWithLeaderAndMembers_Should_Change_GuildLeader()
        {
            // arrange
            var sut = (TestGuild)GuildFake.Complete().Generate();
            var monitor = sut.Monitor();
            var expectedLeader = sut.GetVice();
            var expectedVice = sut.GetLeader();

            // act
            sut.DemoteLeader();
            var actualLeader = sut.GetLeader();
            var actualVice = sut.GetVice();

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().Contain(new[] { expectedLeader, expectedVice });
            actualLeader.Should().NotBeNull()
                .And.BeOfType<TestMember>()
                .And.Be(expectedLeader);
            actualVice.Should().NotBeNull()
                .And.BeOfType<TestMember>()
                .And.Be(expectedVice);
            actualLeader.Should().NotBe(actualVice);
            actualLeader.IsGuildLeader.Should().BeTrue();
            actualVice.IsGuildLeader.Should().BeFalse();

            monitor.AssertPropertyNotChanged(
                nameof(Guild.Id),
                nameof(Guild.Name));
            monitor.AssertCollectionNotChanged(sut.Members, sut.Invites);
        }

        [Fact]
        public void DemoteLeader_GuildWithLeaderOnly_Should_Change_Nothing()
        {
            // arrange
            var sut = (TestGuild)GuildFake.LeaderOnly().Generate();
            var leader = sut.GetLeader();
            var monitor = sut.Monitor();
            var nullVice = sut.GetVice();

            // act
            sut.DemoteLeader();

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().Contain(leader).And.NotContain(nullVice);
            sut.GetLeader().Should().NotBeNull()
                .And.BeOfType<TestMember>()
                .And.Be(leader)
                .And.NotBe(nullVice);
            sut.GetVice().Should().NotBeNull()
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
            var sut = (TestGuild)GuildFake.NoMembers().Generate();
            var monitor = sut.Monitor();
            var nullLeader = sut.GetLeader();
            var nullVice = sut.GetVice();

            // act
            sut.DemoteLeader();

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestGuild>();
            sut.Members.Should().HaveCount(0).And.NotContain(new[] { nullLeader, nullVice });
            sut.GetLeader().Should().NotBeNull()
                .And.BeOfType<NullMember>()
                .And.Be(nullVice)
                .And.Be(nullLeader);
            sut.GetVice().Should().NotBeNull()
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
