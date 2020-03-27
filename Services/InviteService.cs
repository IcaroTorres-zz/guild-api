using DataAccess.Context;
using DataAccess.Entities;
using Domain.DTOs;
using Domain.Models;
using Domain.Models.NullEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class InviteService : BaseService, IInviteService
    {
        public InviteService(ApiContext context) : base(context) { }

        public InviteModel Get(Guid id, bool readOnly = false)
        {
            var query = Query<Invite>(x => x.Id == id)
                .Include(x => x.Member.Memberships)
                .Include(x => x.Guild.Members);

            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault() is Invite inviteEntity
                ? new InviteModel(inviteEntity)
                : new NullInvite();
        }
        public IReadOnlyList<InviteModel> List(InviteDto payload) => Query<Invite>(
            x => (payload.MemberId == Guid.Empty || x.MemberId == payload.MemberId)
              && (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => new InviteModel(x))
                .ToList();

        public InviteModel InviteMember(InviteDto payload)
        {
            var guildEntity = GetByKeys<Guild>((object)payload.GuildId);
            var guildModel = guildEntity is Guild ? new GuildModel(guildEntity) : new NullGuild();
            var memberEntity = GetByKeys<Member>((object)payload.MemberId);
            var memberModel = memberEntity is Member ? new MemberModel(memberEntity) : new NullMember();
            var newInvite = guildModel.Invite(memberModel);
            newInvite.Entity = Insert(newInvite);
            return newInvite;

            //if (guildModel.Invite(memberModel) is InviteModel newInvite)
            //{
            //    newInvite.Entity = Insert(newInvite);
            //    return newInvite;
            //}

            //var badInvite = new NullInvite();
            //(badInvite.ValidationResult as BadRequestValidationResult)
            //    .AddValidationErrors(nameof(Invite), $"{nameof(Member)} is invalid or already in inviting {nameof(Guild)}.");
            //return badInvite;
        }
        public InviteModel Accept(Guid id) => Get(id).BeAccepted();
        public InviteModel Decline(Guid id) => Get(id).BeDeclined();
        public InviteModel Cancel(Guid id) => Get(id).BeCanceled();
        public InviteModel Delete(Guid id)
        {
            var inviteModel = Get(id);
            inviteModel.Entity = Remove(inviteModel);
            return inviteModel;
        }
    }
}
