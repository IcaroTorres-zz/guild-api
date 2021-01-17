using Business.Dtos;
using Business.Responses;
using Business.Usecases.Members.CreateMember;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tests.Business.Usecases.Members.CreateMember;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Tests.Helpers.Builders;
using Xunit;

namespace Tests.Business.Usecases.Guilds.CreateGuild
{
    [Trait("Business", "Handler")]
    public class CreateMemberHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Succeed_With_ValidCommandAsync()
        {
            // arrange
            var command = CreateMemberCommandFake.Valid();
            var member = MemberFake.WithoutGuild().Generate();
            var repository = MemberRepositoryMockBuilder.Create().Insert(output: member).Build();
            var mapper = MapperConfig.Configuration.CreateMapper();
            var sut = new CreateMemberHandler(repository, mapper);

            // act
            var response = await sut.Handle(command, default);

            // assert
            response.Should().NotBeNull().And.BeOfType<ApiCreatedResult>();
            response.Success.Should().BeTrue();
            response.Errors.Should().BeEmpty();
            response.As<ApiCreatedResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Data.Should().NotBeNull().And.BeOfType<MemberDto>();
            response.Data.As<MemberDto>().Id.Should().Be(member.Id);
            response.Data.As<MemberDto>().Name.Should().Be(member.Name);
        }
    }
}
