using Business.Usecases.Guilds.UpdateGuild;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.UpdateGuild
{
    [Trait("Business", "Validator")]
    public class UpdateGuildValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = UpdateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(true, command.Id, command.Name)
                .Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MasterId)
                .IsGuildMember(true, command.MasterId, command.Id)
                .Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
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
            var command = UpdateGuildCommandFake.InvalidByEmptyId().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
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
            var command = UpdateGuildCommandFake.InvalidByEmptyName().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
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
            var command = UpdateGuildCommandFake.InvalidByEmptyMasterId().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create().Build();
            var memberRepository = MemberRepositoryMockBuilder.Create().Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_Guild_CanChangeName_False()
        {
            // arrange
            var command = UpdateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(false, command.Id, command.Name)
                .Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MasterId)
                .IsGuildMember(true, command.MasterId, command.Id)
                .Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_NewMaster_ExistsWithId_False()
        {
            // arrange
            var command = UpdateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(true, command.Id, command.Name)
                .Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(false, command.MasterId)
                .IsGuildMember(true, command.MasterId, command.Id)
                .Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_IsGuildMember_False()
        {
            // arrange
            var command = UpdateGuildCommandFake.Valid().Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.Id)
                .CanChangeName(true, command.Id, command.Name)
                .Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .ExistsWithId(true, command.MasterId)
                .IsGuildMember(false, command.MasterId, command.Id)
                .Build();

            var sut = new UpdateGuildValidator(guildRepository, memberRepository)
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
