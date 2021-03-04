using Application.Common.Results;
using Application.Invites.Queries.GetInvite;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Invites.Queries.GetInvite
{
    [Trait("Application", "Handler")]
    public class GetInviteHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedInvite = InviteFake.ValidWithStatus(InviteStatuses.Accepted).Generate();
            var command = GetInviteCommandFake.Valid(expectedInvite.Id).Generate();
            var repository = InviteRepositoryMockBuilder.Create()
                .GetByIdSuccess(input: command.Id, output: expectedInvite).Build();
            var sut = new GetInviteHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<TestInvite>();
            result.Data.As<Invite>().Id.Should().Be(expectedInvite.Id);
            result.Data.As<Invite>().Guild.Id.Should().Be(expectedInvite.GuildId.Value);
            result.Data.As<Invite>().Member.Id.Should().Be(expectedInvite.MemberId.Value);
        }
    }
}
