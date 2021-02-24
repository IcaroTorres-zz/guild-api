using Domain.Common;
using Domain.Enums;
using Domain.Nulls;
using Domain.States.Invites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Models
{
    public class Invite : EntityModel<Invite>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private InviteStatuses _status;
        private Guid? _guildId;
        private Guild _guild;
        private Guid? _memberId;
        private Member _member;
        private InviteState _state;

        [JsonConstructor] protected Invite() { }

        internal Invite(Guild guild, Member member)
        {
            Id = Guid.NewGuid();
            Guild = guild;
            GuildId = Guild.Id;
            Member = member;
            MemberId = Member.Id;
            Status = InviteStatuses.Pending;
        }

        public static readonly NullInvite Null = new NullInvite();

        public virtual Invite BeAccepted()
        {
            return State.BeAccepted();
        }

        public virtual Invite BeDenied()
        {
            return State.BeDenied();
        }

        public virtual Invite BeCanceled()
        {
            return State.BeCanceled();
        }

        internal virtual Invite ChangeState(InviteState state)
        {
            State = state;
            Status = State.Status;
            return this;
        }

        public virtual InviteStatuses Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                _status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        public virtual Guid? MemberId
        {
            get => _memberId;
            protected set
            {
                if (_memberId == value) return;
                _memberId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberId)));
            }
        }

        public virtual Guid? GuildId
        {
            get => _guildId;
            protected set
            {
                if (_guildId == value) return;
                _guildId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GuildId)));
            }
        }

        public virtual Guild Guild
        {
            get => _guild ??= Guild.Null;
            protected set
            {
                if (_guild == value) return;
                _guild = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
            }
        }

        public virtual Member Member
        {
            get => _member ??= Member.Null;
            protected set
            {
                if (_member == value) return;
                _member = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
            }
        }

        protected virtual InviteState State
        {
            get => _state ??= InviteState.NewState(this, Guild, Member, Status);
            set => _state = value;
        }

        [NotMapped]
        public IEnumerable<Invite> InvitesToCancel => Guild.Invites.Where(x => x.Status == InviteStatuses.Pending &&
                                                                               x.MemberId == Member.Id &&
                                                                               x.GuildId == Guild.Id &&
                                                                               x.Id != Id).ToList();
    }
}