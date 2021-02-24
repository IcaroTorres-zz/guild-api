using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Invites.Commands.AcceptInvite
{
    public class AcceptInviteCommand : UpdateCommand<Invite, InviteResponse>
    {
    }
}