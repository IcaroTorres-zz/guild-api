using Application.Common.Responses;
using Application.Common.Results;
using Application.Guilds.Queries.ListGuild;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Guilds.Queries.ListGuild
{
    [Trait("Application", "Handler")]
    public class ListGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedPage = new Random().Next(1, 5);
            var expectedPages = new Random().Next(expectedPage, expectedPage + 1);
            var expectedPageSize = new Random().Next(5, 10);
            var command = ListGuildCommandFake.Valid(pageSize: expectedPageSize, page: expectedPage).Generate();
            var repository = GuildRepositoryMockBuilder.Create().Paginate(
                pageSize: command.PageSize,
                page: command.Page,
                totalItems: command.PageSize * expectedPages).Build();
            var sut = new ListGuildHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<PagedResponse<Guild>>();
            result.Data.As<PagedResponse<Guild>>().Items.Should().NotBeEmpty()
                .And.AllBeAssignableTo<Guild>()
                .And.HaveCount(expectedPageSize)
                .And.HaveCount(command.PageSize);
            result.Data.As<PagedResponse<Guild>>().PageSize.Should().Be(expectedPageSize)
                .And.Be(command.PageSize);
            result.Data.As<PagedResponse<Guild>>().Page.Should().Be(expectedPage)
                .And.Be(command.Page);
            result.Data.As<PagedResponse<Guild>>().Pages.Should().Be(expectedPages);
        }
    }
}
