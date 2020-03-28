using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Repositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class InviteService : IInviteService
    {
        private readonly IGuildRepository GuildRepository;
        private readonly IMemberRepository MemberRepository;
        private readonly IInviteRepository InviteRepository;

        public InviteService(IGuildRepository guilds,
            IMemberRepository members,
            IInviteRepository invites)
        {
            InviteRepository = invites;
            GuildRepository = guilds;
            MemberRepository = members;
        }

        public InviteModel Get(Guid id, bool readOnly = false)
        {
            return ModelFactory.ConstructWith<InviteModel, Invite>(InviteRepository.Get(id, readOnly));
        }
        public IReadOnlyList<InviteModel> List(InviteDto payload)
        {
            return InviteRepository.Query(x =>
                (payload.MemberId == Guid.Empty || x.MemberId == payload.MemberId) &&
                (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => new InviteModel(x))
                .ToList();
        }

        public InviteModel InviteMember(InviteDto payload)
        {
            var guildEntity = GuildRepository.Get(payload.GuildId);
            var guildModel = ModelFactory.ConstructWith<GuildModel, Guild>(guildEntity);
            var memberEntity = MemberRepository.Get(payload.MemberId);
            var memberModel = ModelFactory.ConstructWith<MemberModel, Member>(memberEntity);
            var newInvite = guildModel.Invite(memberModel);
            if (newInvite is NullInvite)
            {
                newInvite.Entity.GetType()
                    .GetProperty(nameof(Invite.Id))
                    .SetValue(newInvite.Entity, Guid.Empty);
            }
            newInvite.Entity = InviteRepository.Insert(newInvite);
            return newInvite;
        }
        public InviteModel Accept(Guid id) => Get(id).BeAccepted();
        public InviteModel Decline(Guid id) => Get(id).BeDeclined();
        public InviteModel Cancel(Guid id) => Get(id).BeCanceled();
        public InviteModel Delete(Guid id)
        {
            var inviteModel = Get(id);
            inviteModel.Entity = InviteRepository.Remove(inviteModel);
            return inviteModel;
        }
    }
}
