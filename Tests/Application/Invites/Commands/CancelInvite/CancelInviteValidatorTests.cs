using Application.Invites.Commands.CancelInvite;
using Domain.Enums;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Application.Invites.Commands.CancelInvite
{
    [Trait("Application", "Validator")]
    public class CancelInviteValidatorTests
    {
        [Fact]
        public void Should_Succeed_Cancel()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus().Generate();
            var command = PatchInviteCommandFake.CancelValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new CancelInviteValidator(inviteRepository)
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
        public void Should_Fail_Cancel_By_Empty_Id()
        {
            // arrange
            var command = PatchInviteCommandFake.CancelInvalidByEmptyId().Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().Build();
            var sut = new CancelInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Cancel_By_Invite_NotFound()
        {
            // arrange
            var invite = InviteFake.NullObject().Generate();
            var command = PatchInviteCommandFake.CancelValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdFail(command.Id).Build();
            var sut = new CancelInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Cancel_By_Invite_NotPending()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus(InviteStatuses.Canceled).Generate();
            var command = PatchInviteCommandFake.CancelValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new CancelInviteValidator(inviteRepository)
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
