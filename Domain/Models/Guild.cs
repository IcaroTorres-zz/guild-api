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

        [JsonConstructor]
        protected Guild()
        {
            Members = new HashSet<Member>();
            Invites = new HashSet<Invite>();
        }

        public Guild(string name, Member member) : this()
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
            if (!(member is INullObject) && !Members.Contains(member))
            {
                var invite = new Invite(this, member);
                if (Invites.Add(invite))
                {
                    CollectionChanged?.Invoke(Invites, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, invite));
                }
            }
            return this;
        }

        public virtual Guild RemoveMember(Member member)
        {
            if (!(member is INullObject) && Members.Remove(member))
            {
                member.State.Leave();
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, member));
            }
            return this;
        }

        internal virtual Guild AddMember(Member newMember)
        {
            if (!(newMember is INullObject) && Members.Add(newMember))
            {
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newMember));
            }
            return this;
        }

        public virtual Guild Promote(Member member)
        {
            if (Members.FirstOrDefault(x => x.Id == member.Id) is { } memberInGuild)
            {
                GetLeader().State.BeDemoted();
                memberInGuild.State.BePromoted();
            }

            return this;
        }

        public virtual Guild DemoteLeader()
        {
            return Members.Count > 1 ? Promote(GetVice()) : this;
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

        public virtual HashSet<Member> Members { get; protected set; }
        public virtual HashSet<Invite> Invites { get; protected set; }

        private static bool FilterGuildLeader(Member x) => x.IsGuildLeader;
        public Member GetLeader() => Members.SingleOrDefault(FilterGuildLeader) ?? Member.Null;

        private static TimeSpan OrderActiveMembershipByDuration(Member x) => x.GetActiveMembership().GetDuration();
        public Member GetVice() => Members.OrderByDescending(OrderActiveMembershipByDuration)
                                          .FirstOrDefault(x => x.Id != GetLeader()?.Id && !x.IsGuildLeader) ?? Member.Null;

        private static DateTime OrderInviteByCreation(Invite x) => x.CreatedDate;
        public Invite GetLatestInvite() => Invites.OrderByDescending(OrderInviteByCreation).FirstOrDefault() ?? Invite.Null;
    }
}