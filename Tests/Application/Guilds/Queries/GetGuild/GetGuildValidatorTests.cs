using Application.Guilds.Queries.GetGuild;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Application.Guilds.Queries.GetGuild
{
    [Trait("Application", "Validator")]
    public class GetGuildValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = GetGuildCommandFake.Valid().Generate();
            var sut = new GetGuildValidator
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
            var command = GetGuildCommandFake.InvalidByEmptyId().Generate();
            var sut = new GetGuildValidator
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
