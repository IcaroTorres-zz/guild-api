using Business.Responses;
using Business.Usecases.Guilds.CreateGuild;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.CreateGuild
{
    [Trait("Business", "Handler")]
    public class CreateGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var master = MemberFake.GuildLeader().Generate();
            var command = CreateGuildCommandFake.Valid(master.Id);
            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(
                    x => x.GetForGuildOperationsSuccess(master.Id, master)
                          .Update(output: master)
                          .Build())
                .SetupMemberships(
                    x => x.Insert(output: master.ActiveMembership)
                          .Update(output: master.LastFinishedMembership)
                          .Build())
                .SetupGuilds(x => x.Insert(output: master.Guild).Build())
                .SetupInvites(x => x.Insert(output: master.Guild.GetLatestInvite()).Build())
                .Build();
            var sut = new CreateGuildHandler(unit);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiCreatedResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Data.Should().NotBeNull().And.BeOfType<Guild>();
            result.Data.As<Guild>().Id.Should().Be(master.Guild.Id);
            result.Data.As<Guild>().Name.Should().Be(master.Guild.Name);
        }
    }
}
