using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Invites.Queries.GetInvite
{
    public class GetInviteCommand : QueryItemCommand<Invite, InviteResponse>
    {
    }
}