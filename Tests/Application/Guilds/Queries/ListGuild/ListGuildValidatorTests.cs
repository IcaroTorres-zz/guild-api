using Application.Guilds.Queries.ListGuild;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Application.Guilds.Queries.ListGuild
{
    [Trait("Application", "Validator")]
    public class ListGuildValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = ListGuildCommandFake.Valid().Generate();
            var sut = new ListGuildValidator
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
            var command = ListGuildCommandFake.InvalidByPage().Generate();
            var sut = new ListGuildValidator
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
            var command = ListGuildCommandFake.InvalidByPageSize().Generate();
            var sut = new ListGuildValidator
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
