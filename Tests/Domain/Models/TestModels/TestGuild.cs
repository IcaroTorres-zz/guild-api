using Domain.Common;
using Domain.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Tests.Domain.Models.TestModels
{
    public class TestGuild : Guild, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public override Guild ChangeName(string newName)
        {
            if (base.Name != newName) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            return base.ChangeName(newName);
        }

        public override Invite InviteMember(Member member, IModelFactory factory)
        {
            var invite = base.InviteMember(member, factory);
            if (!(invite is INullObject))
            {
                CollectionChanged?.Invoke(Invites, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, invite));
            }
            return invite;
        }

        public override Membership RemoveMember(Member member)
        {
            var finishedMembership = base.RemoveMember(member);
            if (!(finishedMembership is INullObject))
            {
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, member));
            }
            return finishedMembership;
        }

        internal override Member AddMember(Member newMember)
        {
            var addedMember = base.AddMember(newMember);
            if (!(addedMember is INullObject))
            {
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newMember));
            }
            return addedMember;
        }
    }
}