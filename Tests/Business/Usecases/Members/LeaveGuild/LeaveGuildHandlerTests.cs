using Business.Dtos;
using Business.Responses;
using Business.Usecases.Members.LeaveGuild;
using Domain.Models.Nulls;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.LeaveGuild
{
    [Trait("Business", "Handler")]
    public class LeaveGuildHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand_GuildLeader()
        {
            // arrange
            var leavingMaster = MemberFake.GuildLeader().Generate();
            var expectedNewLeader = leavingMaster.Guild.Vice;
            var expectedFinishedMembership = leavingMaster.ActiveMembership;
            var command = PatchMemberCommandFake.LeaveGuildValid(leavingMaster.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(command.Id, leavingMaster)
                .Update(leavingMaster, leavingMaster)
                .Update(expectedNewLeader, expectedNewLeader)
                .Build();
            var membershipRepository = MembershipRepositoryMockBuilder.Create()
                .Update(expectedFinishedMembership, expectedFinishedMembership).Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new LeaveGuildHandler(memberRepository, membershipRepository, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<MemberDto>();
            result.Data.As<MemberDto>().Id.Should().Be(leavingMaster.Id);
            result.Data.As<MemberDto>().IsGuildLeader.Should().BeFalse()
                .And.Be(!expectedNewLeader.IsGuildLeader);
            result.Data.As<MemberDto>().Guild.Should().BeNull();
            expectedFinishedMembership.Should().NotBeOfType<NullMembership>();
            expectedFinishedMembership.Id.Should().Be(leavingMaster.LastFinishedMembership.Id);
            expectedFinishedMembership.ModifiedDate.Should().NotBeNull()
                .And.Be(leavingMaster.LastFinishedMembership.ModifiedDate);
        }

        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand_GuildMember()
        {
            // arrange
            var leavingMember = MemberFake.GuildMember().Generate();
            var expectedUnchangedLeader = leavingMember.Guild.Leader;
            var expectedFinishedMembership = leavingMember.ActiveMembership;
            var command = PatchMemberCommandFake.LeaveGuildValid(leavingMember.Id).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetForGuildOperationsSuccess(command.Id, leavingMember)
                .Update(leavingMember, leavingMember)
                .Update(expectedUnchangedLeader, expectedUnchangedLeader)
                .Build();
            var membershipRepository = MembershipRepositoryMockBuilder.Create()
                .Update(expectedFinishedMembership, expectedFinishedMembership).Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new LeaveGuildHandler(memberRepository, membershipRepository, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<MemberDto>();
            result.Data.As<MemberDto>().Id.Should().Be(leavingMember.Id);
            result.Data.As<MemberDto>().IsGuildLeader.Should().BeFalse()
                .And.Be(!expectedUnchangedLeader.IsGuildLeader);
            result.Data.As<MemberDto>().Guild.Should().BeNull();
            expectedFinishedMembership.Should().NotBeOfType<NullMembership>();
            expectedFinishedMembership.Id.Should().Be(leavingMember.LastFinishedMembership.Id);
            expectedFinishedMembership.ModifiedDate.Should().NotBeNull()
                .And.Be(leavingMember.LastFinishedMembership.ModifiedDate);
        }
    }
}
