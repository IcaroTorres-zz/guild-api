using Application.Common.Results;
using Application.Guilds.Commands.CreateGuild;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Guilds.Commands.CreateGuild
{
    [Trait("Application", "Handler")]
    public class CreateGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var leader = MemberFake.GuildLeader().Generate();
            var command = CreateGuildCommandFake.Valid(leader.Id);
            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(
                    x => x.GetForGuildOperationsSuccess(leader.Id, leader)
                          .Update(output: leader)
                          .Build())
                .SetupMemberships(
                    x => x.Insert(output: leader.GetActiveMembership())
                          .Update(output: leader.GetLastFinishedMembership())
                          .Build())
                .SetupGuilds(x => x.Insert(output: leader.Guild).Build())
                .SetupInvites(x => x.Insert(output: leader.Guild.GetLatestInvite()).Build())
                .Build();
            var sut = new CreateGuildHandler(unit);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessCreatedResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Data.Should().NotBeNull().And.BeOfType<Guild>();
            result.Data.As<Guild>().Id.Should().Be(leader.Guild.Id);
            result.Data.As<Guild>().Name.Should().Be(leader.Guild.Name);
        }
    }
}
