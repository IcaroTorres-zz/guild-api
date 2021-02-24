using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Invites.Commands.CancelInvite
{
    public class CancelInviteCommand : UpdateCommand<Invite, InviteResponse>
    {
    }
}