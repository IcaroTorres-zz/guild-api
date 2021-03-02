using Application.Common.Responses;
using Application.Common.Results;
using Application.Members.Queries.ListMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Queries.ListMember
{
    [Trait("Application", "Handler")]
    public class ListMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedPage = new Random().Next(1, 5);
            var expectedPages = new Random().Next(expectedPage, expectedPage + 1);
            var expectedPageSize = new Random().Next(5, 10);
            var command = ListMemberCommandFake.Valid(expectedPageSize, expectedPage).Generate();
            var totalItems = command.PageSize * expectedPages;
            var repository = MemberRepositoryMockBuilder.Create().Paginate(command, totalItems).Build();
            var sut = new ListMemberHandler(repository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<PagedResponse<Member>>();
            result.Data.As<PagedResponse<Member>>().Items.Should().NotBeEmpty()
                .And.AllBeAssignableTo<Member>()
                .And.HaveCount(expectedPageSize)
                .And.HaveCount(command.PageSize)
                .And.Match(x => x.All(y => y.Name.Contains(command.Name, StringComparison.OrdinalIgnoreCase) ||
                                           y.GuildId == command.GuildId));
            result.Data.As<PagedResponse<Member>>().PageSize.Should().Be(expectedPageSize)
                .And.Be(command.PageSize);
            result.Data.As<PagedResponse<Member>>().Page.Should().Be(expectedPage)
                .And.Be(command.Page);
            result.Data.As<PagedResponse<Member>>().Pages.Should().Be(expectedPages);
        }
    }
}
