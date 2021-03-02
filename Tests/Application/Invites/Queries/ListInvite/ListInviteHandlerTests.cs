using Application.Common.Responses;
using Application.Common.Results;
using Application.Invites.Queries.ListInvite;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Invites.Queries.ListInvite
{
    [Trait("Application", "Handler")]
    public class ListInviteHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedPage = new Random().Next(1, 5);
            var expectedPages = new Random().Next(expectedPage, expectedPage + 1);
            var expectedPageSize = new Random().Next(5, 10);
            var command = ListInviteCommandFake.Valid(expectedPageSize, expectedPage).Generate();
            var repository = InviteRepositoryMockBuilder.Create().Paginate(
                pageSize: command.PageSize,
                page: command.Page,
                totalItems: command.PageSize * expectedPages).Build();
            var sut = new ListInviteHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<PagedResponse<Invite>>();
            result.Data.As<PagedResponse<Invite>>().Items.Should().NotBeEmpty()
                .And.AllBeAssignableTo<Invite>()
                .And.HaveCount(expectedPageSize)
                .And.HaveCount(command.PageSize);
            result.Data.As<PagedResponse<Invite>>().PageSize.Should().Be(expectedPageSize)
                .And.Be(command.PageSize);
            result.Data.As<PagedResponse<Invite>>().Page.Should().Be(expectedPage)
                .And.Be(command.Page);
            result.Data.As<PagedResponse<Invite>>().Pages.Should().Be(expectedPages);
        }
    }
}
