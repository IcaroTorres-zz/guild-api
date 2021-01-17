using Business.Usecases.Invites.AcceptInvite;
using Domain.Enums;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Invites.AcceptInvite
{
    [Trait("Business", "Validator")]
    public class AcceptInviteValidatorTests
    {
        [Fact]
        public void Should_Succeed_Accept()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus().Generate();
            var command = PatchInviteCommandFake.AcceptValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new AcceptInviteValidator(inviteRepository)
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
        public void Should_Fail_Accept_By_Empty_Id()
        {
            // arrange
            var command = PatchInviteCommandFake.AcceptInvalidByEmptyId().Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().Build();
            var sut = new AcceptInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Accept_By_Invite_NotFound()
        {
            // arrange
            var invite = InviteFake.NullObject().Generate();
            var command = PatchInviteCommandFake.AcceptValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdFail(command.Id).Build();
            var sut = new AcceptInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Accept_By_Invite_NotPending()
        {
            // arrange
            var invite = InviteFake.ValidWithStatus(InviteStatuses.Accepted).Generate();
            var command = PatchInviteCommandFake.AcceptValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new AcceptInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Accept_By_Guild_NotFound()
        {
            // arrange
            var invite = InviteFake.InvalidWithoutGuild().Generate();
            var command = PatchInviteCommandFake.AcceptValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new AcceptInviteValidator(inviteRepository)
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_Accept_By_Member_NotFound()
        {
            // arrange
            var invite = InviteFake.InvalidWithoutMember().Generate();
            var command = PatchInviteCommandFake.AcceptValid(invite.Id).Generate();
            var inviteRepository = InviteRepositoryMockBuilder.Create().GetByIdSuccess(output: invite).Build();
            var sut = new AcceptInviteValidator(inviteRepository)
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
