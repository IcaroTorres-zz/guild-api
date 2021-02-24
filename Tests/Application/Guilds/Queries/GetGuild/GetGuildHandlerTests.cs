using Application.Common.Results;
using Application.Guilds.Queries.GetGuild;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Guilds.Queries.GetGuild
{
    [Trait("Application", "Handler")]
    public class GetGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            int otherMembersCount = new Random().Next(1, 10);
            var expectedGuild = GuildFake.WithGuildLeaderAndMembers(otherMembersCount: otherMembersCount).Generate();
            var command = GetGuildCommandFake.Valid(expectedGuild.Id).Generate();
            var repository = GuildRepositoryMockBuilder.Create()
                .GetByIdSuccess(input: command.Id, output: expectedGuild).Build();
            var sut = new GetGuildHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<Guild>();
            result.Data.As<Guild>().Id.Should().Be(expectedGuild.Id);
            result.Data.As<Guild>().GetLeader().Id.Should().Be(expectedGuild.GetLeader().Id);
            result.Data.As<Guild>().Members.Should().NotBeEmpty().And.HaveCount(otherMembersCount + 1);
        }
    }
}
