using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Invites.Commands.DenyInvite
{
    public class DenyInviteCommand : UpdateCommand<Invite, InviteResponse>
    {
    }
}