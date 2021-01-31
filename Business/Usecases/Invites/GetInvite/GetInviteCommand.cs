using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Invites.GetInvite
{
    public class GetInviteCommand : QueryItemCommand<Invite, InviteDto>
    {
    }
}