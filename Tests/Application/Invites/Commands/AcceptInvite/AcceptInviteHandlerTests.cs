using Application.Common.Results;
using Application.Invites.Commands.AcceptInvite;
using Domain.Enums;
using Domain.Models;
using Domain.Nulls;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.Proxies;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Invites.Commands.AcceptInvite
{
    [Trait("Application", "Handler")]
    public class AcceptInviteHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            const int canceledCount = 2;
            var invitedMember = MemberFake.GuildLeader().Generate();
            var promotedMember = invitedMember.GetGuild().GetVice();
            var invitingGuild = GuildFake.Complete().Generate();
            var acceptedInvite = InviteFake.ValidToAcceptWithInvitesToCancel(canceledCount, invitingGuild, invitedMember).Generate();
            var command = PatchInviteCommandFake.AcceptValid(acceptedInvite.Id).Generate();
            var canceledInvites = acceptedInvite.GetInvitesToCancel().ToArray();

            var startedMembership = MembershipFake.Active(invitingGuild, invitedMember).Generate();
            var finishedMembership = invitedMember.GetActiveMembership();

            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(x => x.Update(input: invitedMember, output: invitedMember)
                                    .Update(input: promotedMember, output: promotedMember).Build())
                .SetupMemberships(x => x.Insert(output: startedMembership).Update(output: finishedMembership).Build())
                .SetupInvites(x =>
                {
                    x.GetForAcceptOperationSuccess(input: command.Id, output: acceptedInvite)
                     .Update(output: acceptedInvite);

                    foreach (var i in canceledInvites) x.Update(i, i);

                    return x.Build();
                }).Build();
            var factory = ModelFactoryMockBuilder.Create()
                .CreateMembership(invitingGuild, invitedMember, startedMembership).Build();
            var sut = new AcceptInviteHandler(unit, factory);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<SuccessResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<SuccessResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<InviteTestProxy>();
            result.Data.As<Invite>().Id.Should().Be(acceptedInvite.Id);
            result.Data.As<Invite>().Status.Should().Be(InviteStatuses.Accepted)
                .And.Be(acceptedInvite.Status);

            invitedMember.Should().NotBeOfType<NullMember>();
            invitedMember.IsGuildLeader.Should().BeFalse();
            invitedMember.GetGuild().Should().Be(invitingGuild);

            invitingGuild.Should().NotBeOfType<NullGuild>();
            invitingGuild.Members.Should().Contain(invitedMember);

            finishedMembership.ModifiedDate.Should().NotBeNull()
                .And.Be(invitedMember.GetLastFinishedMembership().ModifiedDate);
            canceledInvites.Should().HaveCount(canceledCount)
                .And.OnlyContain(x => x.Status == InviteStatuses.Canceled);
            promotedMember.Should().NotBeNull().And.BeOfType<MemberTestProxy>();
            promotedMember.IsGuildLeader.Should().Be(!invitedMember.IsGuildLeader);
        }
    }
}
