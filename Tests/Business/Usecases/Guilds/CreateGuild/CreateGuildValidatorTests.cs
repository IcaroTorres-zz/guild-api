using Business.Usecases.Guilds.CreateGuild;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.CreateGuild
{
    [Trait("Business", "Validator")]
    public class CreateGuildValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = CreateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder
                .Create().ExistsWithName(false, command.Name).Build();
            var memberRepository = MemberRepositoryMockBuilder
                .Create().ExistsWithId(true, command.MasterId).Build();

            var sut = new CreateGuildValidator(guildRepository, memberRepository)
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
            var command = CreateGuildCommandFake.InvalidByEmptyName().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new CreateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Empty_MasterId()
        {
            // arrange
            var command = CreateGuildCommandFake.InvalidByEmptyMasterId().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new CreateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Guild_ExistsWithName_True()
        {
            // arrange
            var command = CreateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder
                .Create().ExistsWithName(true, command.Name).Build();
            var memberRepository = MemberRepositoryMockBuilder
                .Create().ExistsWithId(true, command.MasterId).Build();

            var sut = new CreateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Master_ExistsWithId_False()
        {
            // arrange
            var command = CreateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder
                .Create().ExistsWithName(false, command.Name).Build();
            var memberRepository = MemberRepositoryMockBuilder
                .Create().ExistsWithId(false, command.MasterId).Build();

            var sut = new CreateGuildValidator(guildRepository, memberRepository)
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
