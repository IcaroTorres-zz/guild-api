using Domain.Common;
using Domain.Nulls;
using Domain.States.Members;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Domain.Models
{
    public class Member : EntityModel<Member>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private string _name;
        private bool _isGuildLeader;
        private Guid? _guildId;
        private Guild _guild;
        private MemberState _state;

        [JsonConstructor]
        protected Member()
        {
            Memberships = new HashSet<Membership>();
        }

        public Member(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static readonly NullMember Null = new NullMember();

        internal virtual Member ChangeState(MemberState state)
        {
            State = state;
            IsGuildLeader = State.IsGuildLeader;
            Guild = State.Guild;
            if (Guild is INullObject) GuildId = null;
            else GuildId = State.Guild.Id;

            return this;
        }

        public virtual Member ChangeName(string newName)
        {
            Name = newName;
            return this;
        }

        internal virtual Member ActivateMembership(Guild guild)
        {
            var membership = new Membership(guild, this);
            if (!(guild is INullObject) && Memberships.Add(membership))
            {
                CollectionChanged?.Invoke(Memberships, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, membership));
            }
            return this;
        }

        public virtual string Name
        {
            get => _name;
            protected set
            {
                if (_name != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    _name = value;
                }
            }
        }

        public virtual bool IsGuildLeader
        {
            get => _isGuildLeader;
            protected set
            {
                if (_isGuildLeader != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGuildLeader)));
                    _isGuildLeader = value;
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

        internal virtual MemberState State
        {
            get => _state ??= MemberState.NewState(this);
            set => _state = value;
        }

        public virtual HashSet<Membership> Memberships { get; protected set; }

        private static bool activeMembershipFilter(Membership x) => x.ModifiedDate == null && x != Membership.Null;
        public Membership GetActiveMembership() => Memberships.SingleOrDefault(activeMembershipFilter) ?? Membership.Null;

        private static bool finishedMembershipFilter(Membership x) => x.ModifiedDate != null && x != Membership.Null;
        public Membership GetLastFinishedMembership() => Memberships.Where(finishedMembershipFilter)
                                                                    .OrderByDescending(x => x.ModifiedDate)
                                                                    .FirstOrDefault() ?? Membership.Null;
    }
}