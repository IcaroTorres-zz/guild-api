using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class InviteService : IInviteService
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IInviteRepository _inviteRepository;
        private readonly ModelFactory _modelFactory;
        private readonly IValidator<Invite> _inviteValidator;

        public InviteService(IGuildRepository guilds,
            IMemberRepository members,
            IInviteRepository invites,
            ModelFactory factory,
            IValidator<Invite> validator)
        {
            _inviteRepository = invites;
            _guildRepository = guilds;
            _memberRepository = members;
            _modelFactory = factory;
            _inviteValidator = validator;
        }

        public InviteModel Get(Guid id, bool readOnly = false)
        {
            return (InviteModel) _modelFactory
                .Create(_inviteRepository.Get(id, readOnly))
                .ApplyValidator(_inviteValidator);
        }
        public IReadOnlyList<InviteModel> List(InviteDto payload)
        {
            return _inviteRepository.Query(x =>
                (payload.MemberId == Guid.Empty || x.MemberId == payload.MemberId) &&
                (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => _modelFactory.Create(x))
                .ToList();
        }

        public InviteModel InviteMember(InviteDto payload)
        {
            var guildEntity = _guildRepository.Get(payload.GuildId);
            var guildModel = _modelFactory.Create(guildEntity);
            var memberEntity = _memberRepository.Get(payload.MemberId);
            var memberModel = _modelFactory.Create(memberEntity);
            var inviteModel = guildModel.Invite(memberModel);
            inviteModel.Entity = _inviteRepository.Insert(inviteModel.ApplyValidator(_inviteValidator));
            return inviteModel;
        }
        public InviteModel Accept(Guid id) => (InviteModel) Get(id).BeAccepted().ApplyValidator(_inviteValidator);
        public InviteModel Decline(Guid id) => (InviteModel)Get(id).BeDeclined().ApplyValidator(_inviteValidator);
        public InviteModel Cancel(Guid id) => (InviteModel) Get(id).BeCanceled().ApplyValidator(_inviteValidator);
        public InviteModel Delete(Guid id)
        {
            var inviteModel = Get(id);
            inviteModel.Entity = _inviteRepository.Remove(inviteModel);
            return inviteModel;
        }
    }
}
