using Application.Common.Results;
using Application.Members.Commands.ChangeMemberName;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Commands.ChangeMemberName
{
    [Trait("Application", "Handler")]
    public class ChangeMemberNameHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedMember = MemberFake.WithoutGuild().Generate();
            var command = PatchMemberCommandFake.ChangeMemberNameValid(
                id: expectedMember.Id,
                name: expectedMember.Name).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetByIdSuccess(command.Id, expectedMember)
                .Update(expectedMember, expectedMember)
                .Build();
            var sut = new ChangeMemberNameHandler(memberRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<TestMember>();
            result.Data.As<Member>().Name.Should().Be(expectedMember.Name);
            result.Data.As<Member>().Id.Should().Be(expectedMember.Id);
        }
    }
}
