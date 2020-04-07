using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Commands.Members
{
	public class CreateMemberCommand : IRequest<ApiResponse<Member>>, ITransactionalCommand
	{
		public string Name { get; set; }
	}
}