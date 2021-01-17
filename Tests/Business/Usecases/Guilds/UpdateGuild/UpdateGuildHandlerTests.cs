using Business.Dtos;
using Business.Responses;
using Business.Usecases.Guilds.UpdateGuild;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Business.Usecases.Guilds.UpdateGuild;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.UpdateGuild
{
    [Trait("Business", "Handler")]
    public class UpdateGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var otherMembersCount = new Random().Next(1, 5);
            var expectedGuild = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
            var expectedLeader = expectedGuild.Vice;
            var expectedVice = expectedGuild.Leader;
            var command = UpdateGuildCommandFake.Valid(
                id: expectedGuild.Id,
                masterId: expectedLeader.Id,
                name: expectedGuild.Name).Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .GetByIdSuccess(command.Id, expectedGuild)
                .Update(expectedGuild, expectedGuild).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .Update(expectedLeader, expectedLeader)
                .Update(expectedVice, expectedVice)
                .Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new UpdateGuildHandler(guildRepository, memberRepository, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<GuildDto>();
            result.Data.As<GuildDto>().Name.Should().Be(expectedGuild.Name);
            result.Data.As<GuildDto>().Id.Should().Be(expectedGuild.Id);
            result.Data.As<GuildDto>().Leader.Id.Should().Be(expectedLeader.Id);
            result.Data.As<GuildDto>().Members.Should().NotBeEmpty()
                .And.HaveCount(otherMembersCount + 1)
                .And.Contain(x => x.Id == expectedLeader.Id)
                .And.Contain(x => x.Id == expectedVice.Id);
        }
    }
}
