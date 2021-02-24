using Domain.Common;
using Domain.Nulls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Domain.Models
{
    public class Guild : EntityModel<Guild>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

		private string _name;

		[JsonConstructor] protected Guild() { }

        public Guild(string name, Member member)
        {
            Id = Guid.NewGuid();
            Name = name;
            InviteMember(member);
            GetLatestInvite().BeAccepted();
        }

        public static readonly NullGuild Null = new NullGuild();

        public virtual Guild ChangeName(string newName)
        {
            Name = newName;
            return this;
        }

        public virtual Guild InviteMember(Member member)
        {
            if (!Members.Contains(member))
            {
                var invite = member.ConfirmGuildInvite(this);
                Invites.Add(invite);
                CollectionChanged?.Invoke(Invites, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, invite));
            }
            return this;
        }

        internal virtual Guild RemoveMember(Member member)
        {
            if (Members.Contains(member))
            {
                if (member.IsGuildLeader) member.TransferLeadership(GetVice());
                Members.Remove(member);
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, member));
				member.LeaveGuild();
			}
            return this;
        }

        internal virtual Guild AddMember(Member newMember)
        {
            if (!Members.Contains(newMember))
            {
                Members.Add(newMember);
				CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newMember));
            }
            return this;
        }

        public virtual Guild Promote(Member member)
        {
			Members.SingleOrDefault(x => x.Id == member.Id)?.ReceiveLeadership(GetLeader());

			return this;
		}

        public virtual Guild DemoteLeader()
		{
			if (Members.Count > 1)
			{
                GetLeader().TransferLeadership(GetVice());
            }

			return this;
		}

        internal virtual Membership ConfirmMembership(Member member)
        {
            return new Membership(this, member);
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

        public virtual ICollection<Member> Members { get; protected set; } = new List<Member>();
        public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();

        private static bool FilterGuildLeader(Member x) => x.IsGuildLeader;
        public Member GetLeader() => Members.SingleOrDefault(FilterGuildLeader) ?? Member.Null;

        private static TimeSpan OrderActiveMembershipByDuration(Member x) => x.ActiveMembership.GetDuration();
        public Member GetVice() => Members.OrderByDescending(OrderActiveMembershipByDuration)
                                          .FirstOrDefault(x => x.Id != GetLeader()?.Id && !x.IsGuildLeader) ?? Member.Null;

        private static DateTime OrderInviteByCreation(Invite x) => x.CreatedDate;
        public Invite GetLatestInvite() => Invites.OrderByDescending(OrderInviteByCreation).FirstOrDefault() ?? Invite.Null;
    }
}