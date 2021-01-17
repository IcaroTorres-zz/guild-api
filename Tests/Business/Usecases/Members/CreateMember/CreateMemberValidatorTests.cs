using Business.Usecases.Members.CreateMember;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.CreateMember
{
    [Trait("Business", "Validator")]
    public class CreateMemberValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = CreateMemberCommandFake.Valid().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithName(false, command.Name).Build();

            var sut = new CreateMemberValidator(memberRepository)
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
        public void Should_Fail_By_Empty_Name()
        {
            // arrange
            var command = CreateMemberCommandFake.InvalidByEmptyName().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new CreateMemberValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Member_ExistsWithName_True()
        {
            // arrange
            var command = CreateMemberCommandFake.Valid().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithName(true, command.Name).Build();

            var sut = new CreateMemberValidator(memberRepository)
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
