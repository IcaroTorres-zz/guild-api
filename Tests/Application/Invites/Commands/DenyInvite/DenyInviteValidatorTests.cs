using Application.Invites.Commands.DenyInvite;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Invites.Commands.DenyInvite
{
    [Trait("Application", "Validator")]
    public class DenyInviteValidatorTests
    {
        [Fact]
        public void Should_Succeed_Deny()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus().Generate();
            var command = PatchInviteCommandFake.DenyValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new DenyInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_Deny_By_Empty_Id()
        {
            // arrange
            var command = PatchInviteCommandFake.DenyInvalidByEmptyId().Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().Build();
            var sut = new DenyInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Deny_By_Invite_NotFound()
        {
            // arrange
            var invite = Invite.Null;
            var command = PatchInviteCommandFake.DenyValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdFail(command.Id).Build();
            var sut = new DenyInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Deny_By_Invite_NotPending()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus(InviteStatuses.Denied).Generate();
            var command = PatchInviteCommandFake.DenyValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new DenyInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }
    }
}
