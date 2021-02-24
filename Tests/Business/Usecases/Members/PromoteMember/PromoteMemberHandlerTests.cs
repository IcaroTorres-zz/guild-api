using Business.Responses;
using Business.Usecases.Members.PromoteMember;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.PromoteMember
{
    [Trait("Business", "Handler")]
    public class PromoteMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var promotedMember = MemberFake.GuildMember().Generate();
            var guild = promotedMember.Guild;
            var expectedDemotedMember = promotedMember.Guild.GetLeader();
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
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<Member>();
            result.Data.As<Member>().Id.Should().Be(promotedMember.Id);
            result.Data.As<Member>().IsGuildLeader.Should().BeTrue()
                .And.Be(!expectedDemotedMember.IsGuildLeader);
            result.Data.As<Member>().Guild.Should().NotBeNull();
            result.Data.As<Member>().Guild.Id.Should().Be(guild.Id);
        }
    }
}
