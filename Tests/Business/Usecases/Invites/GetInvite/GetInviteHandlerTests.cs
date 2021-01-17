using Business.Dtos;
using Business.Responses;
using Business.Usecases.Invites.GetInvite;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Invites.GetInvite
{
    [Trait("Business", "Handler")]
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
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new GetInviteHandler(repository, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<InviteDto>();
            result.Data.As<InviteDto>().Id.Should().Be(expectedInvite.Id);
            result.Data.As<InviteDto>().Guild.Id.Should().Be(expectedInvite.GuildId.Value);
            result.Data.As<InviteDto>().Member.Id.Should().Be(expectedInvite.MemberId.Value);
        }
    }
}
