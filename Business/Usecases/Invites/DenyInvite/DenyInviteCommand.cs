using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Invites.DenyInvite
{
    public class DenyInviteCommand : UpdateCommand<Invite, InviteDto>
    {
    }
}