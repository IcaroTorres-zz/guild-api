using Application.Common.Results;
using Application.Members.Commands.PromoteMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Members.Commands.PromoteMember
{
    [Trait("Application", "Handler")]
    public class PromoteMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var promotedMember = MemberFake.GuildMember().Generate();
            var guild = promotedMember.GetGuild();
            var expectedDemotedMember = promotedMember.GetGuild().GetLeader();
            var command = PatchMemberCommandFake.PromoteMemberValid(promotedMember.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(command.Id, promotedMember)
                .Update(promotedMember, promotedMember)
                .Update(expectedDemotedMember, expectedDemotedMember)
                .Build();
            var sut = new PromoteMemberHandler(memberRepository);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<TestMember>();
            result.Data.As<Member>().Id.Should().Be(promotedMember.Id);
            result.Data.As<Member>().IsGuildLeader.Should().BeTrue()
                .And.Be(!expectedDemotedMember.IsGuildLeader);
            result.Data.As<Member>().GetGuild().Should().NotBeNull();
            result.Data.As<Member>().GetGuild().Id.Should().Be(guild.Id);
        }
    }
}
