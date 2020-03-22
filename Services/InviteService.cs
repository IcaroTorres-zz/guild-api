using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Validations;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;
using Domain.Models;
using Domain.Models.NullEntities;

namespace Services
{
    public class InviteService : BaseRepository, IInviteService
    {
        public InviteService(ApiContext context) : base(context) { }

        public InviteModel Get(Guid id, bool readOnly = false)
        {
            var query = Query<Invite>(i => i.Id == id)
                .Include(i => i.Member.Memberships)
                .Include(i => i.Guild.Members);

            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault() is Invite inviteEntity
                ? new InviteModel(inviteEntity)
                : new NullInvite();
        }
        public IReadOnlyList<InviteModel> List(InviteDto payload)
        => Query<Invite>(i => (payload.MemberId == Guid.Empty || i.MemberId == payload.MemberId) 
                           && (payload.GuildId == Guid.Empty || i.GuildId == payload.GuildId), readOnly: true)
            .Take(payload.Count)
            .Select(i => new InviteModel(i))
            .ToList();
        public InviteModel InviteMember(InviteDto payload)
        {
            var guildModel = new GuildModel(GetGuild(payload.GuildId));
            var memberModel = new MemberModel(GetMember(payload.MemberId));
            if (guildModel.Invite(memberModel) is InviteModel newInvite)
            {
                newInvite.Entity = Insert(newInvite);
                return newInvite;
            }

            var badInvite = new NullInvite();
            (badInvite.ValidationResult as BadRequestValidationResult)
                .AddValidationErrors(nameof(Invite),$"{nameof(Member)} is invalid or already in inviting {nameof(Guild)}.");
            return badInvite;
        }
        public InviteModel Accept(Guid id) => Get(id).BeAccepted();
        public InviteModel Decline(Guid id) => Get(id).BeDeclined();
        public InviteModel Cancel(Guid id) => Get(id).BeCanceled();
        public InviteModel Delete(Guid id)
        {
            var inviteModel = Get(id);
            inviteModel.Entity = Remove<Invite>(inviteModel);
            return inviteModel;
        }
    }
}
