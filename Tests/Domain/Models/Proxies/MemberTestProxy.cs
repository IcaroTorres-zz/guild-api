using Domain.Common;
using Domain.Models;
using Domain.States.Members;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Tests.Domain.Models.Proxies
{
    public class MemberTestProxy : Member, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal override Member ChangeState(MemberState newState)
        {
            if (base.GetGuild() != newState.Guild)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
            }

            return base.ChangeState(newState);
        }

        public override Member ChangeName(string newName)
        {
            if (base.Name != newName) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            return base.ChangeName(newName);
        }

        internal override Membership ActivateMembership(Guild guild, IModelFactory factory)
        {
            var membership = base.ActivateMembership(guild, factory);
            if (!(membership is INullObject))
            {
                CollectionChanged?.Invoke(Memberships, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, membership));
            }
            return membership;
        }

        public override bool IsGuildLeader
        {
            get => base.IsGuildLeader; protected set
            {
                if (base.IsGuildLeader != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGuildLeader)));
                    base.IsGuildLeader = value;
                }
            }
        }

        public override Guid? GuildId
        {
            get => base.GuildId; protected set
            {
                if (base.GuildId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GuildId)));
                    base.GuildId = value;
                }
            }
        }
    }
}