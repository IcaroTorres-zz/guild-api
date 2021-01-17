using Business.Dtos;
using Business.Responses;
using Business.Usecases.Invites.AcceptInvite;
using Domain.Enums;
using Domain.Models.Nulls;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Tests.Business.Usecases.Invites;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.AcceptInvite
{
    [Trait("Business", "Handler")]
    public class AcceptInviteHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var canceledCount = new Random().Next(1, 5);
            var acceptedInvite = InviteFake.ValidToAcceptWithInvitesToCancel(canceledCount).Generate();
            var command = PatchInviteCommandFake.AcceptValid(acceptedInvite.Id).Generate();
            var canceledInvites = acceptedInvite.InvitesToCancel;

            var invitedMember = acceptedInvite.Member;
            var invitingGuild = acceptedInvite.Guild;
            var startedMembership = MembershipFake.Active().Generate();
            var finishedMembership = invitedMember.ActiveMembership;

            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(x => x.Update(input: invitedMember, output: invitedMember).Build())
                .SetupMemberships(x => x.Insert(output: startedMembership).Update(output: finishedMembership).Build())
                .SetupInvites(x =>
                {
                    x.GetForAcceptOperationSuccess(input: command.Id, output: acceptedInvite)
                     .Update(output: acceptedInvite);

                    foreach(var i in canceledInvites) x.Update(i, i);

                    return x.Build();
                }).Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new AcceptInviteHandler(unit, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<InviteDto>();
            result.Data.As<InviteDto>().Id.Should().Be(acceptedInvite.Id);
            result.Data.As<InviteDto>().Status.Should().Be(InviteStatuses.Accepted)
                .And.Be(acceptedInvite.Status);

            invitedMember.Should().NotBeOfType<NullMember>();
            invitedMember.IsGuildLeader.Should().BeFalse();
            invitedMember.Guild.Should().Be(invitingGuild);

            invitingGuild.Should().NotBeOfType<NullGuild>();
            invitingGuild.Members.Should().Contain(invitedMember);

            finishedMembership.ModifiedDate.Should().NotBeNull()
                .And.Be(invitedMember.LastFinishedMembership.ModifiedDate);
            canceledInvites.Should().HaveCount(canceledCount)
                .And.OnlyContain(x => x.Status == InviteStatuses.Canceled);
        }
    }
}
