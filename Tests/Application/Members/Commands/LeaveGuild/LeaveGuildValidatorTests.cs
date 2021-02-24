using Application.Members.Commands.LeaveGuild;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Commands.LeaveGuild
{
    [Trait("Application", "Validator")]
    public class LeaveGuildValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var command = PatchMemberCommandFake.LeaveGuildValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetByIdSuccess(member.Id, member).Build();
            var sut = new LeaveGuildValidator(memberRepository)
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
        public void Should_Fail_By_Empty_Id()
        {
            // arrange
            var command = PatchMemberCommandFake.LeaveGuildInvalidByEmptyId().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();
            var sut = new LeaveGuildValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_NotFound()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var command = PatchMemberCommandFake.LeaveGuildValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetByIdFail(command.Id).Build();
            var sut = new LeaveGuildValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_Without_Guild()
        {
            // arrange
            var member = MemberFake.WithoutGuild().Generate();
            var command = PatchMemberCommandFake.LeaveGuildValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetByIdSuccess(member.Id, member).Build();
            var sut = new LeaveGuildValidator(memberRepository)
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
