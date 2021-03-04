using Domain.Common;
using Domain.Models;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Tests.Domain.Models.TestModels
{
    public class TestMember : Member, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal override Membership ActivateMembership(Guild guild, IModelFactory factory)
        {
            var membership = base.ActivateMembership(guild, factory);
            if (!(membership is INullObject))
            {
                CollectionChanged?.Invoke(Memberships, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, membership));
            }
            return membership;
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

        public override Guild Guild
        {
            get => base.Guild; protected set
            {
                if (base.Guild != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
                    base.Guild = value;
                }
            }
        }
    }
}