using Domain.Common;
using Domain.Enums;
using Domain.Nulls;
using Domain.States.Invites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                if (_status != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                    _status = value;
                }
            }
        }

        public virtual Guid? MemberId
        {
            get => _memberId;
            protected set
            {
                if (_memberId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberId)));
                    _memberId = value;
                }
            }
        }

        public virtual Guid? GuildId
        {
            get => _guildId;
            protected set
            {
                if (_guildId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GuildId)));
                    _guildId = value;
                }
            }
        }

        public virtual Guild Guild
        {
            get => _guild ??= Guild.Null;
            protected set
            {
                if (_guild != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
                    _guild = value;
                }
            }
        }

        public virtual Member Member
        {
            get => _member ??= Member.Null;
            protected set
            {
                if (_member != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                    _member = value;
                }
            }
        }

        internal virtual InviteState State
        {
            get => _state ??= InviteState.NewState(this, Guild, Member, Status);
            set => _state = value;
        }

        public HashSet<Invite> GetInvitesToCancel() => Guild.Invites
            .Where(x => x.Status == InviteStatuses.Pending &&
                        x.MemberId == Member.Id &&
                        x.GuildId == Guild.Id &&
                        x.Id != Id).ToHashSet();
    }
}