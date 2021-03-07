using Application.Common.Results;
using Application.Guilds.Commands.CreateGuild;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
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
            var command = CreateGuildCommandFake.Valid(leader.Id).Generate();
            var expectedNewGuild = leader.GetGuild();
            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(
                    x => x.GetForGuildOperationsSuccess(leader.Id, leader)
                          .Update(output: leader)
                          .Build())
                .SetupMemberships(
                    x => x.Insert(output: leader.GetActiveMembership())
                          .Update(output: leader.GetLastFinishedMembership())
                          .Build())
                .SetupGuilds(x => x.Insert(output: leader.GetGuild()).Build())
                .SetupInvites(x => x.Insert(output: leader.GetGuild().GetLatestInvite()).Build())
                .Build();
            var factory = ModelFactoryMockBuilder.Create().CreateGuild(command.Name, leader, expectedNewGuild).Build();
            var sut = new CreateGuildHandler(unit, factory);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessCreatedResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Data.Should().NotBeNull().And.BeOfType<TestGuild>().And.Be(expectedNewGuild);
        }
    }
}
