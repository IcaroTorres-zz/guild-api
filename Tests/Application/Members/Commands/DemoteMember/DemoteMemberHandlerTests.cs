using Application.Common.Results;
using Application.Members.Commands.DemoteMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Commands.DemoteMember
{
    [Trait("Application", "Handler")]
    public class DemoteMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var guild = GuildFake.Complete().Generate();
            var demotedLeader = guild.GetLeader();
            var expectedNewLeader = guild.GetVice();
            var command = PatchMemberCommandFake.DemoteMemberValid(demotedLeader.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(command.Id, demotedLeader)
                .Update(demotedLeader, demotedLeader)
                .Update(expectedNewLeader, expectedNewLeader)
                .Build();
            var sut = new DemoteMemberHandler(memberRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<TestMember>();
            result.Data.As<Member>().Id.Should().Be(demotedLeader.Id);
            result.Data.As<Member>().IsGuildLeader.Should().BeFalse()
                .And.Be(!expectedNewLeader.IsGuildLeader);
            result.Data.As<Member>().Guild.Should().NotBeNull();
            result.Data.As<Member>().Guild.Id.Should().Be(guild.Id);
        }
    }
}
