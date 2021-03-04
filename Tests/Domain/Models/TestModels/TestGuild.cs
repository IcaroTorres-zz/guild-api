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

        public override Invite InviteMember(Member member, IModelFactory factory)
        {
            var invite = base.InviteMember(member, factory);
            if (!(invite is INullObject))
            {
                CollectionChanged?.Invoke(Invites, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, invite));
            }
            return invite;
        }

        public override Member RemoveMember(Member member)
        {
            var removedMember = base.RemoveMember(member);
            if (!(removedMember is INullObject))
            {
                CollectionChanged?.Invoke(Members, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedMember));
            }
            return removedMember;
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

        public override string Name
        {
            get => base.Name; protected internal set
            {
                if (base.Name != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    base.Name = value;
                }
            }
        }
    }
}