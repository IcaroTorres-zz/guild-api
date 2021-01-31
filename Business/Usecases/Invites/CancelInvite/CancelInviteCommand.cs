using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Invites.CancelInvite
{
    public class CancelInviteCommand : UpdateCommand<Invite, InviteDto>
    {
    }
}