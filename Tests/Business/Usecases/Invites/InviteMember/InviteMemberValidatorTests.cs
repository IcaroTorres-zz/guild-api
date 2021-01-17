using Business.Usecases.Invites.InviteMember;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Invites.InviteMember
{
    [Trait("Business", "Validator")]
    public class InviteMemberValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = InviteMemberCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.GuildId).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MemberId)
                .IsGuildMember(false, command.MemberId, command.GuildId)
                .Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_By_Empty_GuildId()
        {
            // arrange
            var command = InviteMemberCommandFake.InvalidByEmptyGuildId().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Empty_MemberId()
        {
            // arrange
            var command = InviteMemberCommandFake.InvalidByEmptyMemberId().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Guild_ExistsWithId_False()
        {
            // arrange
            var command = InviteMemberCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(false, command.GuildId).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MemberId)
                .IsGuildMember(false, command.MemberId, command.GuildId)
                .Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_ExistsWithId_False()
        {
            // arrange
            var command = InviteMemberCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.GuildId).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(false, command.MemberId)
                .IsGuildMember(false, command.MemberId, command.GuildId)
                .Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_IsGuildMember_True()
        {
            // arrange
            var command = InviteMemberCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.GuildId).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MemberId)
                .IsGuildMember(true, command.MemberId, command.GuildId)
                .Build();

            var sut = new InviteMemberValidator(memberRepository, guildRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }
    }
}
