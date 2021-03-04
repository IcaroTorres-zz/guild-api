using Application.Common.Results;
using Application.Guilds.Commands.UpdateGuild;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Guilds.Commands.UpdateGuild
{
    [Trait("Application", "Handler")]
    public class UpdateGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedGuild = GuildFake.Valid().Generate();
            var expectedMembersCount = expectedGuild.Members.Count;
            var expectedLeader = expectedGuild.GetVice();
            var expectedVice = expectedGuild.GetLeader();
            var command = UpdateGuildCommandFake.Valid(
                id: expectedGuild.Id,
                leaderId: expectedLeader.Id,
                name: expectedGuild.Name).Generate();
            var guildRepository = GuildRepositoryMockBuilder.Create()
                .GetByIdSuccess(command.Id, expectedGuild)
                .Update(expectedGuild, expectedGuild).Build();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .Update(expectedLeader, expectedLeader)
                .Update(expectedVice, expectedVice)
                .Build();
            var sut = new UpdateGuildHandler(guildRepository, memberRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<TestGuild>();
            result.Data.As<Guild>().Name.Should().Be(expectedGuild.Name);
            result.Data.As<Guild>().Id.Should().Be(expectedGuild.Id);
            result.Data.As<Guild>().GetLeader().Id.Should().Be(expectedLeader.Id);
            result.Data.As<Guild>().Members.Should().NotBeEmpty()
                .And.HaveCount(expectedMembersCount)
                .And.Contain(x => x.Id == expectedLeader.Id)
                .And.Contain(x => x.Id == expectedVice.Id);
        }
    }
}
