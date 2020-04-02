using Domain.Entities;
using MediatR;

namespace Business.Commands.Members
{
    public class CreateMemberCommand : IRequest<ApiResponse<Member>>
    {
        public string Name { get; set; }
    }
}
