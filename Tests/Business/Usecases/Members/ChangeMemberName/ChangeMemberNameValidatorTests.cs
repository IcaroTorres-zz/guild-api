using Business.Usecases.Members.ChangeMemberName;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.ChangeMemberName
{
    [Trait("Business", "Validator")]
    public class ChangeMemberNameValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = PatchMemberCommandFake.ChangeMemberNameValid().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(true, command.Id, command.Name).Build();

            var sut = new ChangeMemberNameValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.Should().NotBeNull()
                .And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_By_Empty_Id()
        {
            // arrange
            var command = PatchMemberCommandFake.ChangeMemberNameInvalidByEmptyId().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new ChangeMemberNameValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Empty_Name()
        {
            // arrange
            var command = PatchMemberCommandFake.ChangeMemberNameInvalidByEmptyName().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new ChangeMemberNameValidator(memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_MemberName_CanChangeName_False()
        {
            // arrange
            var command = PatchMemberCommandFake.ChangeMemberNameValid().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(false, command.Id, command.Name).Build();

            var sut = new ChangeMemberNameValidator(memberRepository)
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
            var command = PatchMemberCommandFake.ChangeMemberNameValid().Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(false, command.Id)
                .CanChangeName(true, command.Id, command.Name).Build();

            var sut = new ChangeMemberNameValidator(memberRepository)
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
