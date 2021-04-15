using Domain.Models;
using System;
using System.ComponentModel;

namespace Tests.Domain.Models.Proxies
{
    public class MembershipTestProxy : Membership, INotifyPropertyChanged
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
    }
}