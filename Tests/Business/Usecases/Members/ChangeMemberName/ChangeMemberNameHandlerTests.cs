using Business.Dtos;
using Business.Responses;
using Business.Usecases.Members.ChangeMemberName;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Members.ChangeMemberName
{
    [Trait("Business", "Handler")]
    public class ChangeMemberNameHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommand()
        {
            // arrange
            var expectedMember = MemberFake.WithoutGuild().Generate();
            var command = PatchMemberCommandFake.ChangeMemberNameValid(
                id: expectedMember.Id,
                name: expectedMember.Name).Generate();
            var memberRepository = MemberRepositoryMockBuilder.Create()
                .GetByIdSuccess(command.Id, expectedMember)
                .Update(expectedMember, expectedMember)
                .Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new ChangeMemberNameHandler(memberRepository, mapper);

            // act
            var result = await sut.Handle(command, default);

            // assert
            result.Should().NotBeNull().And.BeOfType<ApiResult>();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.As<ApiResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().NotBeNull().And.BeOfType<MemberDto>();
            result.Data.As<MemberDto>().Name.Should().Be(expectedMember.Name);
            result.Data.As<MemberDto>().Id.Should().Be(expectedMember.Id);
        }
    }
}
