using Domain.Common;
using Domain.Nulls;
using Domain.States.Memberships;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Domain.Models
{
    public class Membership : EntityModel<Membership>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _createdDate;
        private DateTime? _modifiedDate;
        private Guid? _memberId;
        private Guid? _guildId;
        private Guild _guild;
        private Member _member;
        private MembershipState _state;

        [JsonConstructor] protected Membership() { }

        internal Membership(Guild guild, Member member)
        {
            Id = Guid.NewGuid();
            Guild = guild;
            GuildId = guild.Id;
            Member = member;
            MemberId = member.Id;
            CreatedDate = DateTime.UtcNow;
        }

        public static readonly NullMembership Null = new NullMembership();

        internal Membership ChangeState(MembershipState state)
        {
            State = state;
            ModifiedDate = state.ModifiedDate;
            return this;
        }

        public virtual TimeSpan GetDuration()
        {
            return (ModifiedDate ?? DateTime.UtcNow).Subtract(CreatedDate);
        }

        public override DateTime CreatedDate
        {
            get => _createdDate;
            protected internal set
            {
                if (_createdDate != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreatedDate)));
                    _createdDate = value;
                }
            }
        }

        public override DateTime? ModifiedDate
        {
            get => _modifiedDate;
            protected internal set
            {
                if (_modifiedDate != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModifiedDate)));
                    _modifiedDate = value;
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

        internal virtual MembershipState State
        {
            get => _state ??= MembershipState.NewState(this, ModifiedDate);
            set => _state = value;
        }
    }
}