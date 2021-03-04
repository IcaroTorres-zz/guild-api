using Domain.Models;
using System;
using System.ComponentModel;

namespace Tests.Domain.Models.TestModels
{
    public class TestMembership : Membership, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public override DateTime CreatedDate
        {
            get => base.CreatedDate; protected internal set
            {
                if (base.CreatedDate != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreatedDate)));
                    base.CreatedDate = value;
                }
            }
        }

        public override DateTime? ModifiedDate
        {
            get => base.ModifiedDate; protected internal set
            {
                if (base.ModifiedDate != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModifiedDate)));
                    base.ModifiedDate = value;
                }
            }
        }

        public override Guid? MemberId
        {
            get => base.MemberId; protected internal set
            {
                if (base.MemberId != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberId)));
                    base.MemberId = value;
                }
            }
        }

        public override Guid? GuildId
        {
            get => base.GuildId; protected internal set
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
            get => base.Guild; protected internal set
            {
                if (base.Guild != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guild)));
                    base.Guild = value;
                }
            }
        }

        public override Member Member
        {
            get => base.Member; protected internal set
            {
                if (base.Member != value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                    base.Member = value;
                }
            }
        }
    }
}