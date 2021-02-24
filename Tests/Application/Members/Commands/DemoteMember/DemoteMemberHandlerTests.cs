using Application.Common.Results;
using Application.Members.Commands.DemoteMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
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
            var demotedMember = MemberFake.GuildLeader().Generate();
            var guild = demotedMember.Guild;
            var expectedLeader = demotedMember.Guild.GetVice();
            var command = PatchMemberCommandFake.DemoteMemberValid(demotedMember.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(command.Id, demotedMember)
                .Update(demotedMember, demotedMember)
                .Update(expectedLeader, expectedLeader)
                .Build();
            var sut = new DemoteMemberHandler(memberRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<Member>();
            result.Data.As<Member>().Id.Should().Be(demotedMember.Id);
            result.Data.As<Member>().IsGuildLeader.Should().BeFalse()
                .And.Be(!expectedLeader.IsGuildLeader);
            result.Data.As<Member>().Guild.Should().NotBeNull();
            result.Data.As<Member>().Guild.Id.Should().Be(guild.Id);
        }
    }
}
