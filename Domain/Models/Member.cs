using Domain.Models.Nulls;
using Domain.Models.States.Members;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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

        [JsonConstructor] protected Member() { }

        public Member(string name)
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
			if (Guild is NullGuild) GuildId = null;
			else GuildId = State.Guild.Id;

            return this;
        }

        public virtual Member ChangeName(string newName)
        {
            Name = newName;
            return this;
        }

        public virtual Member LeaveGuild()
        {
            return State.Leave();
        }

        internal virtual Member ReceiveLeadership(Member currentLeader)
        {
			currentLeader.State.BeDemoted();
            return State.BePromoted();
        }

        internal virtual Member TransferLeadership(Member newLeader)
        {
            newLeader.State.BePromoted();
            return State.BeDemoted();
        }

        internal virtual Member JoinGuild(Guild guild)
        {
            return State.Join(guild);
        }

        internal virtual Invite ConfirmGuildInvite(Guild guild)
        {
            return new Invite(guild, this);
        }

        internal virtual Member ActivateMembership(Guild guild)
        {
            var membership = guild.ConfirmMembership(this);
            Memberships.Add(membership);
            CollectionChanged?.Invoke(Memberships, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, membership));
            return this;
        }

        public virtual string Name
        {
            get => _name;
            protected set
            {
                if (_name == value) return;
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public virtual bool IsGuildLeader
        {
            get => _isGuildLeader;
            protected set
            {
                if (_isGuildLeader == value) return;
                _isGuildLeader = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGuildLeader)));
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

		protected virtual MemberState State
        {
            get => _state ??= MemberState.NewState(this, Guild, IsGuildLeader);
            set => _state = value;
        }

        public virtual ICollection<Membership> Memberships { get; protected set; } = new List<Membership>();

        [NotMapped]
        public Membership ActiveMembership => Memberships.SingleOrDefault(x => x.ModifiedDate == null) ?? Membership.Null;

        [NotMapped]
        public Membership LastFinishedMembership => Memberships.Where(x => x.ModifiedDate != null)
                                                               .OrderByDescending(x => x.ModifiedDate)
                                                               .FirstOrDefault() ?? Membership.Null;
    }
}