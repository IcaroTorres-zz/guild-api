using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities.NullEntities;
using DataAccess.Validations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class InviteService : BaseService, IInviteService
    {
        public InviteService(ApiContext context) : base(context) { }

        public IInvite Get(Guid id, bool readOnly = false)
        {
            var query = Query<Invite>(i => i.Id == id)
                .Include(i => i.Member.Memberships)
                .Include(i => i.Guild.Members);

            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault() ?? new NullInvite();
        }
        public IReadOnlyList<IInvite> List(InviteDto payload) => Query<Invite>(
            i => (payload.MemberId == Guid.Empty || i.MemberId == payload.MemberId)
              && (payload.GuildId == Guid.Empty || i.GuildId == payload.GuildId), 
              readOnly: true).Take(payload.Count).ToList();
        public IInvite InviteMember(InviteDto payload)
        {
            var guild = GetGuild(payload.GuildId);
            var member = GetMember(payload.MemberId);
            if (guild.Invite(member) is Invite newInvite)
            {
                return Insert(newInvite);
            }

            var badInvite = new NullInvite();
            (badInvite.ValidationResult as BadRequestValidationResult)
                .AddValidationErrors(nameof(Invite),$"{nameof(Member)} is invalid or already in inviting {nameof(Guild)}.");
            return badInvite;
        }
        public IInvite Accept(Guid id) => Get(id).BeAccepted();
        public IInvite Decline(Guid id) => Get(id).BeDeclined();
        public IInvite Cancel(Guid id) => Get(id).BeCanceled();
        public IInvite Delete(Guid id) => Remove<Invite>(id);
    }
}
