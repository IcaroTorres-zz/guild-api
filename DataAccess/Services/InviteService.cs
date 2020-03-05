using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Services
{
    public class InviteService : BaseService, IInviteService
    {
        public InviteService(ApiContext context) : base(context) { }

        public IInvite GetInvite(Guid id)
        {
            var included = $"{nameof(Invite.Guild)},{nameof(Invite.Member)}" +
                $",{nameof(Invite.Guild)}.{nameof(Invite.Guild.Members)}" +
                $",{nameof(Invite.Member)}.{nameof(Invite.Member.Memberships)}";

            return Query<Invite>(i => i.Id == id, included: included).SingleOrDefault()
                ?? throw new KeyNotFoundException($"Target {nameof(Invite)} with id '{id}' not found.");
        }

        public IReadOnlyList<IInvite> List(InviteDto payload) => Query<Invite>(
            i => (payload.MemberId == Guid.Empty || i.MemberId == payload.MemberId)
              && (payload.GuildId == Guid.Empty || i.GuildId == payload.GuildId), 
              readOnly: true).Take(payload.Count).ToList();

        public IInvite InviteMember(InviteDto payload)
        {
            var guild = GetGuild(payload.GuildId);

            var member = GetMember(payload.MemberId);

            return guild.Invite(member) ?? throw new ArgumentException("Can not invite member contained in inviting guild.", nameof(payload.MemberId));
        }

        public IInvite Accept(Guid id)
        {
            var invite = GetInvite(id);

            invite.BeAccepted();

            return invite;
        }

        public IInvite Decline(Guid id)
        {
            var invite = GetInvite(id);

            invite.BeDeclined();

            return invite;
        }

        public IInvite Cancel(Guid id)
        {
            var invite = GetInvite(id);

            invite.BeCanceled();

            return invite;
        }

        private IMember GetMember(Guid memberId)
        {
            var keys = new object[] { memberId };
            var navigations = new[] { nameof(Member.Guild) };
            var collections = new[] { nameof(Member.Memberships) };

            return GetWithKeys<Member>(keys, navigations, collections)
                ?? throw new KeyNotFoundException($"Target {nameof(Member)} with id '{memberId}' not found.");
        }

        private IGuild GetGuild(Guid id)
        {
            var keys = new object[] { id };
            var collections = new[] { nameof(Guild.Members), nameof(Guild.Invites) };

            return GetWithKeys<Guild>(keys, collections: collections)
                ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");
        }

        public IInvite Delete(Guid id)
        {
            return Remove<Invite>(id);
        }
    }
}
