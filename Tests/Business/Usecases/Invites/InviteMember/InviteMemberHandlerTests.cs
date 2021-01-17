using Business.Dtos;
using Business.Responses;
using Business.Usecases.Invites.InviteMember;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Business.Usecases.Invites.InviteMember;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.InviteMember
{
    [Trait("Business", "Handler")]
    public class InviteMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var guild = GuildFake.WithGuildLeader().Generate();
            var member = MemberFake.WithoutGuild().Generate();
            var command = InviteMemberCommandFake.Valid(guild.Id, member.Id).Generate();
            var expectedInvite = InviteFake.ValidWithStatus(InviteStatuses.Pending, guild, member).Generate();

            var unit = UnitOfWorkMockBuilder.Create()
                .SetupMembers(x => x.GetForGuildOperationsSuccess(command.MemberId, member).Build())
                .SetupGuilds(x => x.GetForMemberHandlingSuccess(command.GuildId, guild).Build())
                .SetupInvites(x => x.Insert(output: expectedInvite).Build())
                .Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new InviteMemberHandler(unit, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiCreatedResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Data.Should().NotBeNull().And.BeOfType<InviteDto>();
            result.Data.As<InviteDto>().Id.Should().Be(expectedInvite.Id);
            result.Data.As<InviteDto>().Guild.Id.Should().Be(expectedInvite.GuildId.Value).And.Be(guild.Id);
            result.Data.As<InviteDto>().Member.Id.Should().Be(expectedInvite.MemberId.Value).And.Be(member.Id);
        }
    }
}
