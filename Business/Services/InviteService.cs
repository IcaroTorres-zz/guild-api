using DataAccess.Context;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class InviteService : BaseService, IInviteService
    {
        public InviteService(ApiContext context) : base(context) { }

        public InviteModel Get(Guid id, bool readOnly = false)
        {
            var query = Query<Invite>(x => x.Id == id)
                .Include(x => x.Member.Memberships)
                .Include(x => x.Guild.Members)
                .ThenInclude(m => m.Memberships);

            return ModelFactory.ConstructWith<InviteModel, Invite>(
                (readOnly ? query.AsNoTracking() : query).SingleOrDefault());
        }
        public IReadOnlyList<InviteModel> List(InviteDto payload)
        {
            return Query<Invite>(x =>
                (payload.MemberId == Guid.Empty || x.MemberId == payload.MemberId) &&
                (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => new InviteModel(x))
                .ToList();
        }

        public InviteModel InviteMember(InviteDto payload)
        {
            var guildEntity = GetByKeys<Guild>((object)payload.GuildId);
            var guildModel = ModelFactory.ConstructWith<GuildModel, Guild>(guildEntity);
            var memberEntity = GetByKeys<Member>((object)payload.MemberId);
            var memberModel = ModelFactory.ConstructWith<MemberModel, Member>(memberEntity);
            var newInvite = guildModel.Invite(memberModel);
            if (newInvite is NullInvite)
            {
                newInvite.Entity.GetType()
                    .GetProperty(nameof(Invite.Id))
                    .SetValue(newInvite.Entity, Guid.Empty);
            }
            newInvite.Entity = Insert(newInvite);
            return newInvite;
        }
        public InviteModel Accept(Guid id)
        {
            return Get(id).BeAccepted();
        }

        public InviteModel Decline(Guid id)
        {
            return Get(id).BeDeclined();
        }

        public InviteModel Cancel(Guid id)
        {
            return Get(id).BeCanceled();
        }

        public InviteModel Delete(Guid id)
        {
            var inviteModel = Get(id);
            inviteModel.Entity = Remove(inviteModel);
            return inviteModel;
        }
    }
}
