using Application.Common.Results;
using Application.Members.Commands.CreateMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Commands.CreateMember
{
    [Trait("Application", "Handler")]
    public class CreateMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var command = CreateMemberCommandFake.Valid();
            var member = MemberFake.WithoutGuild().Generate();
            var repository = MemberRepositoryMockBuilder.Create().Insert(output: member).Build();
            var sut = new CreateMemberHandler(repository);

            // act
            var response = await sut.Handle(command, default);

            // assert
            response.Should().NotBeNull().And.BeOfType<ApiCreatedResult>();
            response.Success.Should().BeTrue();
            response.Errors.Should().BeEmpty();
            response.As<ApiCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Data.Should().NotBeNull().And.BeOfType<Member>();
            response.Data.As<Member>().Id.Should().Be(member.Id);
            response.Data.As<Member>().Name.Should().Be(member.Name);
        }
    }
}
