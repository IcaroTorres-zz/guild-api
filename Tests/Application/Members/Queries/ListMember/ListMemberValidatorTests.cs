using Application.Members.Queries.ListMember;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Application.Members.Queries.ListMember;
using Tests.Helpers;
using Xunit;

namespace Tests.Business.Usecases.Members.ListMember
{
    [Trait("Application", "Validator")]
    public class ListMemberValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = ListMemberCommandFake.Valid().Generate();
            var sut = new ListMemberValidator
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
        public void Should_Fail_By_Page_LessThan0()
        {
            // arrange
            var command = ListMemberCommandFake.InvalidByPage().Generate();
            var sut = new ListMemberValidator
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_PageSize_LessThan0()
        {
            // arrange
            var command = ListMemberCommandFake.InvalidByPageSize().Generate();
            var sut = new ListMemberValidator
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
