using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Invites.AcceptInvite
{
    public class AcceptInviteCommand : UpdateCommand<Invite, InviteDto>
    {
    }
}