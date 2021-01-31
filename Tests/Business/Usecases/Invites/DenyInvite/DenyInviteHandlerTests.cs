using Business.Responses;
using Business.Usecases.Invites.DenyInvite;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Invites.DenyInvite
{
    [Trait("Business", "Handler")]
    public class DenyInviteHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var deniedInvite = InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();
            var command = PatchInviteCommandFake.DenyValid(deniedInvite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create()
                .GetByIdSuccess(command.Id, deniedInvite)
                .Update(deniedInvite, deniedInvite)
                .Build();
            var sut = new DenyInviteHandler(inviteRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<Invite>();
            result.Data.As<Invite>().Id.Should().Be(deniedInvite.Id);
            result.Data.As<Invite>().Status.Should().Be(InviteStatuses.Denied)
                .And.Be(deniedInvite.Status);
        }
    }
}
