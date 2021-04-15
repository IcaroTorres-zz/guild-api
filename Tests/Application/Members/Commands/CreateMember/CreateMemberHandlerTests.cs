using Application.Common.Results;
using Application.Members.Commands.CreateMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.Proxies;
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
            var command = CreateMemberCommandFake.Valid().Generate();
            var expectedMember = MemberFake.WithoutGuild(command.Name).Generate();
            var repository = MemberRepositoryMockBuilder.Create().Insert(command.Name, expectedMember).Build();
            var factory = ModelFactoryMockBuilder.Create().CreateMember(command.Name, expectedMember).Build();
            var sut = new CreateMemberHandler(repository, factory);

            // act
            var response = await sut.Handle(command, default);

            // assert
            response.Should().NotBeNull().And.BeOfType<SuccessCreatedResult>();
            response.Success.Should().BeTrue();
            response.Errors.Should().BeEmpty();
            response.As<SuccessCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Data.Should().NotBeNull().And.BeOfType<MemberTestProxy>();
            response.Data.As<Member>().Should().Be(expectedMember);
        }
    }
}
