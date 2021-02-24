using Application.Common.Responses;
using Application.Common.Results;
using Application.Memberships.Queries.ListMemberships;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Memberships.Queries.ListMembership
{
    [Trait("Application", "Handler")]
    public class ListMembershipHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedPage = new Random().Next(1, 5);
            var expectedPages = new Random().Next(expectedPage, expectedPage + 1);
            var expectedPageSize = new Random().Next(5, 10);
            var command = ListMembershipCommandFake.Valid(expectedPageSize, expectedPage).Generate();
            var repository = MembershipRepositoryMockBuilder.Create().Paginate(
                pageSize: command.PageSize,
                page: command.Page,
                totalItems: command.PageSize * expectedPages).Build();
            var sut = new ListMembershipHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<PagedResponse<Membership>>();
            result.Data.As<PagedResponse<Membership>>().Items.Should().NotBeEmpty()
                .And.AllBeAssignableTo<Membership>()
                .And.HaveCount(expectedPageSize)
                .And.HaveCount(command.PageSize);
            result.Data.As<PagedResponse<Membership>>().PageSize.Should().Be(expectedPageSize)
                .And.Be(command.PageSize);
            result.Data.As<PagedResponse<Membership>>().Page.Should().Be(expectedPage)
                .And.Be(command.Page);
            result.Data.As<PagedResponse<Membership>>().Pages.Should().Be(expectedPages);
        }
    }
}
