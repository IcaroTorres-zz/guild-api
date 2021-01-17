using Business.Usecases.Members.PromoteMember;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.PromoteMember
{
    [Trait("Business", "Validator")]
    public class PromoteMemberValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var command = PatchMemberCommandFake.PromoteMemberValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(member.Id, member).Build();
            var sut = new PromoteMemberValidator(memberRepository)
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
            var command = PatchMemberCommandFake.PromoteMemberInvalidByEmptyId().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();
            var sut = new PromoteMemberValidator(memberRepository)
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
            var command = PatchMemberCommandFake.PromoteMemberValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsFail(member.Id).Build();
            var sut = new PromoteMemberValidator(memberRepository)
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
            var command = PatchMemberCommandFake.PromoteMemberValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(member.Id, member).Build();
            var sut = new PromoteMemberValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_IsGuildLeader()
        {
            // arrange
            var member = MemberFake.GuildLeader().Generate();
            var command = PatchMemberCommandFake.PromoteMemberValid(member.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(member.Id, member).Build();
            var sut = new PromoteMemberValidator(memberRepository)
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
